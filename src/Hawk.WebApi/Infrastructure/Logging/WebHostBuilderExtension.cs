namespace Hawk.WebApi.Infrastructure.Logging
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    internal static class WebHostBuilderExtension
    {
        internal static IWebHostBuilder ConfigureLogging(this IWebHostBuilder @this) => @this
            .ConfigureLogging((hostingContext, logging) => logging
            .AddConfiguration(hostingContext.Configuration.GetSection("Logging"))
            .AddConsole());
    }
}
