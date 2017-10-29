namespace Hawk.WebApi.Configuration
{
    using Hawk.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal static class Authentication
    {
        internal static IServiceCollection ConfigureIdentityServer(
            this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<AuthenticationConfig>();

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