namespace Hawk.WebApi.Infrastructure.Swagger
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;

    internal static class ApplicationBuilderExtension
    {
        internal static IApplicationBuilder UseSwagger(this IApplicationBuilder @this, IConfiguration configuration)
        {
            var swaggerConfig = configuration
                .GetSection("swagger")
                .Get<SwaggerConfiguration>();

            if (!swaggerConfig.Enabled.GetValueOrDefault(false))
            {
                return @this;
            }

            return @this
                .UseOpenApi();
        }
    }
}
