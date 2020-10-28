namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Hawk.WebApi.Infrastructure.Authentication.Configurations;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using static Hawk.WebApi.Infrastructure.Authentication.Policies;

    internal static class MvcOptionsExtension
    {
        internal static MvcOptions AddAuthorizeFilter(
            this MvcOptions @this,
            IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<AuthConfiguration>();

            if (!authConfig.IsEnabled())
            {
                return @this;
            }

            using var provider = serviceCollection.BuildServiceProvider();
            var authorizationService = provider.GetRequiredService<IAuthorizationService>();

            @this.Filters.Add(new CustomAuthorizeFilter(authorizationService, AdminPolicy));

            return @this;
        }
    }
}
