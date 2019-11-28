namespace Hawk.WebApi.Infrastructure.Swagger
{
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using NSwag;

    using static Hawk.Infrastructure.JsonSettings;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureSwagger(this IServiceCollection @this, IConfiguration configuration)
        {
            var swaggerConfig = configuration
                .GetSection("swagger")
                .Get<SwaggerConfiguration>();

            if (!swaggerConfig.Enabled.GetValueOrDefault(false))
            {
                return @this;
            }

            using var provider = @this.BuildServiceProvider();
            var versionDescriptionProvider = provider.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var apiVersion in versionDescriptionProvider.ApiVersionDescriptions)
            {
                @this.AddSwaggerDocument(config =>
                {
                    config.SerializerSettings = JsonSerializerSettings;
                    config.DocumentName = $"v{apiVersion.GroupName}";
                    config.ApiGroupNames = new[] { apiVersion.GroupName };
                    config.AddSecurity(configuration);
                    config.PostProcess = document => CreateDocument(document, apiVersion);
                });
            }

            return @this;
        }

        private static void CreateDocument(OpenApiDocument document, ApiVersionDescription apiVersion)
        {
            document.Info.Title = "Hawk API";
            document.Info.Version = $"v{apiVersion.GroupName}";
            document.Info.Description = "Hawk is a personal finance control.";
            document.Info.Contact = new OpenApiContact
            {
                Name = "Junior Oliveira",
                Email = "junolive@gmail.com",
            };

            document.Info.License = new OpenApiLicense
            {
                Name = "MIT",
                Url = "https://opensource.org/licenses/MIT",
            };

            if (apiVersion.IsDeprecated)
            {
                document.Info.Description += "Hawk is a personal finance control. THIS API VERSION HAS BEEN DEPRECATED.";
            }
        }
    }
}
