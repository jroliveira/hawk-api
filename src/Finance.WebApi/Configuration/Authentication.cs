namespace Finance.WebApi.Configuration
{
    using Microsoft.AspNetCore.Builder;

    internal static class Authentication
    {
        internal static IApplicationBuilder UseAuthentication(this IApplicationBuilder app)
        {
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:35653",
                RequireHttpsMetadata = false,

                ApiName = "api1"
            });

            return app;
        }
    }
}