namespace Hawk.WebApi.Infrastructure.Api
{
    using Hawk.WebApi.Infrastructure.Authentication;
    using Hawk.WebApi.Infrastructure.Hal;
    using Hawk.WebApi.Infrastructure.Versioning;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using static Hawk.Infrastructure.Serialization.JsonSettings;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureApi(this IServiceCollection @this, IConfiguration configuration)
        {
            @this.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            @this
                .AddResponseCompression()
                .AddResponseCaching()
                .AddCors(options => options.AddPolicy("CorsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()))
                .AddMvcCore(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.AddApiVersionRoutePrefixConvention();
                    options.AddAuthorizeFilter(@this, configuration);
                })
                .AddApiExplorer()
                .AddAuthorization(configuration)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = JsonSerializerSettings.ContractResolver;
                    options.SerializerSettings.Formatting = JsonSerializerSettings.Formatting;
                    options.SerializerSettings.Culture = JsonSerializerSettings.Culture;
                    options.SerializerSettings.NullValueHandling = JsonSerializerSettings.NullValueHandling;
                    options.SerializerSettings.ReferenceLoopHandling = JsonSerializerSettings.ReferenceLoopHandling;
                    options.SerializerSettings.DateTimeZoneHandling = JsonSerializerSettings.DateTimeZoneHandling;
                    options.SerializerSettings.DateFormatHandling = JsonSerializerSettings.DateFormatHandling;

                    foreach (var converter in JsonSerializerSettings.Converters)
                    {
                        options.SerializerSettings.Converters.Add(converter);
                    }
                })
                .AddHal(JsonSerializerSettings);

            return @this;
        }
    }
}
