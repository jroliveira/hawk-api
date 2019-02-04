namespace Hawk.WebApi.IntegrationTest.Infrastructure.Authentication
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class AuthorizedStartup : Startup
    {
        public AuthorizedStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void ConfigureAuthentication(IServiceCollection services) => services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test Scheme";
                options.DefaultChallengeScheme = "Test Scheme";
            })
            .AddTestAuth(o => { });
    }
}
