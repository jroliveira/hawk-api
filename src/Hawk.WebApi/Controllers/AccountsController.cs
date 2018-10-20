namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Account;
    using Hawk.Domain.Queries.Account;
    using Hawk.WebApi.Lib.Validators;
    using Hawk.WebApi.Models;
    using Hawk.WebApi.Models.Account.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiVersion("1")]
    [Route("accounts")]
    public class AccountsController : BaseController
    {
        private readonly IGetByEmailQuery getByEmail;
        private readonly ICreateCommand create;
        private readonly AccountValidator validator;

        public AccountsController(
            IGetByEmailQuery getByEmail,
            ICreateCommand create)
            : this(getByEmail, create, new AccountValidator())
        {
        }

        internal AccountsController(
            IGetByEmailQuery getByEmail,
            ICreateCommand create,
            AccountValidator validator)
        {
            this.getByEmail = getByEmail;
            this.create = create;
            this.validator = validator;
        }

        /// <summary>
        /// Get by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(Account), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            var entity = await this.getByEmail.GetResult(email).ConfigureAwait(false);

            return entity.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                success => success.Match<IActionResult>(
                    account => this.Ok(new Account(account.Email)),
                    () => this.NotFound($"Resource 'accounts' with email {email} could not be found")));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Account), 201)]
        public async Task<IActionResult> Create([FromBody] Models.Account.Post.Account request)
        {
            var validateResult = await this.validator.ValidateAsync(request).ConfigureAwait(false);
            if (!validateResult.IsValid)
            {
                return this.StatusCode(409, validateResult.Errors);
            }

            var inserted = await this.create.Execute(request).ConfigureAwait(false);

            return inserted.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                account => this.Created(account.Email, new Account(account.Email)));
        }
    }
}
