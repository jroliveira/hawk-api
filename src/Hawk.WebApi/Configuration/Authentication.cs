namespace Hawk.WebApi.Configuration
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    internal static class Authentication
    {
        internal static IServiceCollection ConfigureIdentityServer(this IServiceCollection services)
        {
            services
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:35653";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "finance";
                });

            return services;
        }
    }
}