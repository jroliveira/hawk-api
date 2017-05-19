namespace Finance.WebApi.Configuration
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
            services.AddOptions();
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
                .AddMvcCore(config =>
                {
                    // var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    // config.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddApiExplorer()
                .AddJsonFormatters()
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.Formatting = Formatting.Indented;
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            return services;
        }
    }
}
