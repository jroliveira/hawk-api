namespace Hawk.WebApi.Configuration
{
    using System;

    using Hawk.Infrastructure.Logging;
    using Hawk.Infrastructure.Logging.Methods;
    using Hawk.WebApi.Lib;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    internal static class Log
    {
        internal static IApplicationBuilder UseLog(
            this IApplicationBuilder app,
            IConfigurationRoot configuration,
            IHttpContextAccessor accessor)
        {
            if (!Enum.TryParse(configuration["log:level"], out LogLevel level))
            {
                throw new InvalidCastException($"LogLevel {configuration["log:level"]} is not valid.");
            }

            Action<string> logMethod = new DefaultLogMethod(configuration["log:file"]).Write;

            Logger.Init(level, () => accessor.HttpContext.Request.Headers[Constants.Api.ReqId], logMethod);

            return app;
        }
    }
}
