namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using static Hawk.Infrastructure.Guard;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureAuthentication(
            this IServiceCollection @this,
            IConfiguration configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<Configuration>();

            NotNull(authConfig.Authority, "authConfig.Authority", "AuthConfig authority cannot be null.");

            @this
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(opt =>
                {
                    opt.ApiName = "hawk";
                    opt.Authority = authConfig.Authority;
                    opt.RequireHttpsMetadata = false;
                });

            return @this;
        }
    }
}
