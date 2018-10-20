namespace Hawk.WebApi.Configuration
{
    using Hawk.WebApi.Lib;
    using Hawk.WebApi.Lib.Conventions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using Newtonsoft.Json;

    internal static class Api
    {
        internal static IServiceCollection ConfigureApi(this IServiceCollection @this)
        {
            @this.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            @this
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

            @this.AddApiVersioning(opt => opt.ReportApiVersions = true);

            return @this;
        }
    }
}
