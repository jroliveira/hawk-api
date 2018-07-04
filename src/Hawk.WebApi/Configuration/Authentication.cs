namespace Hawk.WebApi.Configuration
{
    using Hawk.WebApi.Lib.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal static class Authentication
    {
        public static IServiceCollection ConfigureIdentityServer(
            this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<Configuration>();

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