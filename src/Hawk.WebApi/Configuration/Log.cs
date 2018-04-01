namespace Hawk.WebApi.Configuration
{
    using System;

    using Hawk.Infrastructure.Logging;
    using Hawk.Infrastructure.Logging.Methods;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    internal static class Log
    {
        public static IServiceCollection ConfigureLog(
            this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            if (!Enum.TryParse(configuration["log:level"], out LogLevel level))
            {
                throw new InvalidCastException($"LogLevel {configuration["log:level"]} is not valid.");
            }

            Action<string> logMethod = new DefaultLogMethod(configuration["log:file"]).Write;

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(provider => new Logger(level, logMethod));

            return services;
        }
    }
}
