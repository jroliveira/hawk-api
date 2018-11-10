namespace Hawk.WebApi.Configuration
{
    using Hawk.WebApi.Lib.Authentication;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using static Hawk.Infrastructure.Guard;

    internal static class Authentication
    {
        internal static IServiceCollection ConfigureIdentityServer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<Configuration>();

            NotNull(authConfig.Authority, "authConfig.Authority", "AuthConfig's authority cannot be null.");

            services
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(opt =>
                {
                    opt.ApiName = "hawk";
                    opt.Authority = authConfig.Authority;
                    opt.RequireHttpsMetadata = false;
                });

            return services;
        }
    }
}
