namespace Hawk.WebApi.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    internal static class Api
    {
        internal static IServiceCollection ConfigureApi(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // Cors
            services.AddCors(options => options.AddPolicy(
                "CorsPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));

            // Config
            services
                .AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters(serializerSettings =>
                {
                    serializerSettings.Formatting = Formatting.Indented;
                    serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            return services;
        }
    }
}
