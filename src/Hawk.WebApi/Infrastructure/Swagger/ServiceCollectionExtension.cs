namespace Hawk.WebApi.Infrastructure.Swagger
{
    using System.Reflection;

    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;

    using Swashbuckle.AspNetCore.Swagger;

    using static System.IO.Path;

    using static Microsoft.Extensions.PlatformAbstractions.PlatformServices;

    internal static class ServiceCollectionExtension
    {
        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = Default.Application.ApplicationBasePath;
                var fileName = $"{typeof(Startup).GetTypeInfo().Assembly.GetName().Name}.xml";

                return Combine(basePath, fileName);
            }
        }

        internal static IServiceCollection ConfigureSwagger(this IServiceCollection @this) => @this
            .AddSwaggerGen(options =>
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
