namespace Hawk.WebApi.Infrastructure.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureApi(this IServiceCollection @this)
        {
            @this.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            @this
                .AddResponseCompression()
                .AddResponseCaching()
                .AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'V")
                .AddCors(options => options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()))
                .AddMvcCore(options => options.Conventions.Add(new ApiVersionRoutePrefixConvention()))
                .AddApiExplorer()
                .AddAuthorization()
                .AddJsonFormatters(serializerSettings =>
                {
                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    serializerSettings.Formatting = Formatting.Indented;
                    serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            @this
                .AddApiVersioning(opt => opt.ReportApiVersions = true);

            return @this;
        }
    }
}
