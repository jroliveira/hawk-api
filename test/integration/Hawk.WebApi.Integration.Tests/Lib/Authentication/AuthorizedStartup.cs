namespace Hawk.WebApi.Integration.Tests.Lib.Authentication
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class AuthorizedStartup : Startup
    {
        public AuthorizedStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void ConfigureAuth(IServiceCollection services) => services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test Scheme";
                options.DefaultChallengeScheme = "Test Scheme";
            })
            .AddTestAuth(o => { });
    }
}
