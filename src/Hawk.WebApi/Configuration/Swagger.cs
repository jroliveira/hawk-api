namespace Hawk.WebApi.Configuration
{
    using System.IO;
    using System.Reflection;

    using Hawk.WebApi.Lib.Swagger;

    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;

    using Swashbuckle.AspNetCore.Swagger;

    internal static class Swagger
    {
        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = $"{typeof(Startup).GetTypeInfo().Assembly.GetName().Name}.xml";

                return Path.Combine(basePath, fileName);
            }
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection @this)
        {
            @this.AddSwaggerGen(options =>
            {
                using (var provider = @this.BuildServiceProvider())
                {
                    var versionDescriptionProvider = provider.GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    }

                    options.OperationFilter<SwaggerDefaultValues>();
                    options.IncludeXmlComments(XmlCommentsFilePath);
                    options.CustomSchemaIds(x => x.FullName);
                }
            });

            return @this;
        }

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info
            {
                Title = "Hawk API",
                Version = description.ApiVersion.ToString(),
                Description = "Hawk is a personal finance control.",
                Contact = new Contact
                {
                    Name = "Junior Oliveira",
                    Email = "junolive@gmail.com",
                },
                License = new License
                {
                    Name = "MIT",
                    Url = "https://opensource.org/licenses/MIT",
                },
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
