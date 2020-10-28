namespace Hawk.WebApi.Infrastructure.Api
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using static Hawk.Infrastructure.Serialization.JsonSettings;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureApi(
            this IServiceCollection @this,
            Action<IMvcCoreBuilder>? mvcCoreBuilderSetup = null,
            Action<MvcOptions>? mvcOptionsSetup = null)
        {
            @this.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var mvcCoreBuilder = @this
                .AddResponseCompression()
                .AddResponseCaching()
                .AddCors(options => options.AddPolicy("CorsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()))
                .AddMvcCore(options =>
                {
                    options.EnableEndpointRouting = false;
                    mvcOptionsSetup?.Invoke(options);
                })
                .AddApiExplorer()
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
                });

            mvcCoreBuilderSetup?.Invoke(mvcCoreBuilder);

            return @this;
        }
    }
}
