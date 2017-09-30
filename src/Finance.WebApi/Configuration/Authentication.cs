namespace Finance.WebApi.Configuration
{
    using Microsoft.AspNetCore.Builder;

    internal static class Authentication
    {
        internal static IApplicationBuilder UseIdentityServer(this IApplicationBuilder app)
        {
            return app;
        }
    }
}