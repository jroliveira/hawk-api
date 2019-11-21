namespace Hawk.WebApi.IntegrationTest.Infrastructure.Authentication
{
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using static System.Threading.Tasks.Task;

    using static Microsoft.AspNetCore.Authentication.AuthenticateResult;

    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        public TestAuthenticationHandler(
            IOptionsMonitor<TestAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync() => FromResult(Success(new AuthenticationTicket(
            new ClaimsPrincipal(this.Options.Identity),
            new AuthenticationProperties(),
            "Test Scheme")));
    }
}
