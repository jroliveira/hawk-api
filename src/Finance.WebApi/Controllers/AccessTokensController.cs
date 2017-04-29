namespace Finance.WebApi.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Finance.Infrastructure.Data.Neo4j.Queries.Account;
    using Finance.WebApi.Models.AccessToken.Post;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [Route("/access-tokens")]
    public class AccessTokensController : Controller
    {
        private readonly JwtIssuerOptions jwtOptions;
        private readonly GetByEmailQuery getByEmail;

        public AccessTokensController(IOptions<JwtIssuerOptions> jwtOptions, GetByEmailQuery getByEmail)
        {
            this.getByEmail = getByEmail;
            this.jwtOptions = jwtOptions.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync([FromForm] Account model)
        {
            var entity = await this.getByEmail.GetResultAsync(model.Email);
            if (entity == null || entity.ValidatePassword(model.Password))
            {
                return this.BadRequest("Invalid credentials");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, entity.Email),
                new Claim(JwtRegisteredClaimNames.Jti, await this.jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(this.jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
            };

            var jwt = new JwtSecurityToken(
                this.jwtOptions.Issuer,
                this.jwtOptions.Audience,
                claims,
                this.jwtOptions.NotBefore,
                this.jwtOptions.Expiration,
                this.jwtOptions.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var token = new Token
            {
                AccessToken = encodedJwt,
                ExpiresIn = this.jwtOptions.ValidFor.TotalSeconds
            };

            return new OkObjectResult(token);
        }

        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}