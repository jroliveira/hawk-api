namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Hawk.WebApi.Infrastructure.Authentication.Configurations;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using static Microsoft.IdentityModel.Logging.IdentityModelEventSource;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureAuthentication(
            this IServiceCollection @this,
            IConfiguration configuration)
        {
            @this.Configure<AuthConfiguration>(configuration.GetSection("authentication"));

            var authConfig = configuration
                .GetSection("authentication")
                .Get<AuthConfiguration>();

            if (!authConfig.IsEnabled())
            {
                return @this;
            }

            ShowPII = true;

            @this
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(opt =>
                {
                    opt.ApiName = "hawk-api";
                    opt.Authority = authConfig.Authority?.Uri;
                    opt.RequireHttpsMetadata = false;
                });

            return @this;
        }
    }
}
