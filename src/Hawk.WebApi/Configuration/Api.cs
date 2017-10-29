namespace Hawk.WebApi.Configuration
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Cors;
    using Microsoft.AspNetCore.Mvc.Cors.Internal;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using Newtonsoft.Json;

    internal static class Api
    {
        internal static IServiceCollection ConfigureApi(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // Cors
            services.TryAddTransient<CorsAuthorizationFilter, CorsAuthorizationFilter>();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.Configure<MvcOptions>(options => options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy")));

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
