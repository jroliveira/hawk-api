namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Hawk.WebApi.Infrastructure.Authentication.Configurations;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;

    internal static class MvcOptionsExtension
    {
        internal static void AddAuthorizeFilter(this MvcOptions @this, IConfiguration configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<AuthConfiguration>();

            if (!authConfig.IsEnabled())
            {
                return;
            }

            @this.Filters.Add(new AuthorizeFilter());
        }
    }
}
