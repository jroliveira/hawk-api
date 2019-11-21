namespace Hawk.WebApi.Infrastructure.Api
{
    using Hawk.WebApi.Infrastructure.Authentication.Configurations;
    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Infrastructure.Hal;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Authorization;
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
                .AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'V")
                .AddCors(options => options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()))
                .AddMvcCore(options =>
                {
                    options.Conventions.Add(new ApiVersionRoutePrefixConvention());

                    var authConfig = configuration
                        .GetSection("authentication")
                        .Get<AuthConfiguration>();

                    if (authConfig.Enabled)
                    {
                        options.Filters.Add(new AuthorizeFilter());
                    }
                })
                .AddApiExplorer()
                .AddAuthorization()
                .AddJsonFormatters(serializerSettings =>
                {
                    serializerSettings.ContractResolver = JsonSerializerSettings.ContractResolver;
                    serializerSettings.Formatting = JsonSerializerSettings.Formatting;
                    serializerSettings.Culture = JsonSerializerSettings.Culture;
                    serializerSettings.NullValueHandling = JsonSerializerSettings.NullValueHandling;
                    serializerSettings.ReferenceLoopHandling = JsonSerializerSettings.ReferenceLoopHandling;
                    serializerSettings.DateTimeZoneHandling = JsonSerializerSettings.DateTimeZoneHandling;
                    serializerSettings.DateFormatHandling = JsonSerializerSettings.DateFormatHandling;

                    foreach (var converter in JsonSerializerSettings.Converters)
                    {
                        serializerSettings.Converters.Add(converter);
                    }

                    serializerSettings.Converters.Add(new TryModelJsonConverter());
                })
                .AddHal(serializerSettings => serializerSettings.Converters.Add(new TryModelJsonConverter()));

            @this
                .AddApiVersioning(opt => opt.ReportApiVersions = true);

            return @this;
        }
    }
}
