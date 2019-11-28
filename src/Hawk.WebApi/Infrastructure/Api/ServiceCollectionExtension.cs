namespace Hawk.WebApi.Infrastructure.Api
{
    using Hawk.WebApi.Infrastructure.Authentication;
    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Infrastructure.Hal;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using static Hawk.Infrastructure.JsonSettings;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureApi(this IServiceCollection @this, IConfiguration configuration)
        {
            @this.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            @this
                .AddResponseCompression()
                .AddResponseCaching()
                .AddApiVersioning(options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "VVV";
                    options.SubstituteApiVersionInUrl = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                })
                .AddCors(options => options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()))
                .AddMvcCore(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Conventions.Add(new ApiVersionRoutePrefixConvention());
                    options.AddAuthorizeFilter(configuration);
                })
                .AddApiExplorer()
                .AddAuthorization()
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

                    options.SerializerSettings.Converters.Add(new TryModelJsonConverter());
                })
                .AddHal(serializerSettings => serializerSettings.Converters.Add(new TryModelJsonConverter()));

            return @this;
        }
    }
}
