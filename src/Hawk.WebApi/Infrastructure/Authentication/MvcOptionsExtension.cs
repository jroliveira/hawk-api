namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Hawk.WebApi.Infrastructure.Authentication.Configurations;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;

    using static Hawk.Infrastructure.Guard;

    internal static class MvcOptionsExtension
    {
        internal static void AddAuthorizeFilter(this MvcOptions @this, IConfiguration configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<AuthConfiguration>();

            if (!authConfig.Enabled.GetValueOrDefault(false))
            {
                return;
            }

            NotNull(authConfig.Authority?.Uri, "authConfig.Authority.Uri", "AuthConfig authority cannot be null.");

            @this.Filters.Add(new AuthorizeFilter());
        }
    }
}
