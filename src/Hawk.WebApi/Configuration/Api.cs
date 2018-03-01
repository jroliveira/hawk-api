namespace Hawk.WebApi.Configuration
{
    using AutoMapper;

    using Hawk.Domain;
    using Hawk.WebApi.Lib;
    using Hawk.WebApi.Lib.Conventions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    internal static class Api
    {
        public static IServiceCollection ConfigureApi(this IServiceCollection services)
        {
            services
                .AddAutoMapper()
                .AddResponseCompression()
                .AddResponseCaching()
                .AddCors(options => options.AddPolicy(
                    Constants.Api.Cors,
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()))
                .AddMvcCore(options => options.Conventions.Add(new ApiVersionRoutePrefixConvention()))
                .AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'V")
                .AddApiExplorer()
                .AddAuthorization()
                .AddJsonFormatters(serializerSettings =>
                {
                    serializerSettings.Formatting = Formatting.Indented;
                    serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddApiVersioning(opt => opt.ReportApiVersions = true);

            return services;
        }
    }
}
