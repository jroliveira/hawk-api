namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Hawk.WebApi.Infrastructure.Authentication.Configurations;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using static Hawk.Infrastructure.Guard;
    using static Microsoft.IdentityModel.Logging.IdentityModelEventSource;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureAuthentication(
            this IServiceCollection @this,
            IConfiguration configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<AuthConfiguration>();

            if (!authConfig.Enabled)
            {
                return @this;
            }

            NotNull(authConfig.Authority, "authConfig.Authority", "AuthConfig authority cannot be null.");

            ShowPII = true;

            @this
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(opt =>
                {
                    opt.ApiName = "hawk";
                    opt.Authority = authConfig.Authority.Uri;
                    opt.RequireHttpsMetadata = false;
                });

            return @this;
        }
    }
}
