namespace Hawk.WebApi.Infrastructure.Logging
{
    using Microsoft.AspNetCore.Builder;

    using Serilog;

    internal static class ApplicationBuilderExtension
    {
        internal static IApplicationBuilder UseLogging(this IApplicationBuilder @this) => @this
            .UseSerilogRequestLogging();
    }
}
