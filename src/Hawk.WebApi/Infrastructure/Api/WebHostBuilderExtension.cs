namespace Hawk.WebApi.Infrastructure.Api
{
    using System.Net;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    using static System.Int32;
    using static System.IO.Directory;

    internal static class WebHostBuilderExtension
    {
        private static IConfiguration? configuration;

        internal static IWebHostBuilder ConfigureApi(this IWebHostBuilder @this) => @this
            .ConfigureAppConfiguration((hostingContext, config) => configuration = config
                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build())
            .UseContentRoot(GetCurrentDirectory())
            .UseKestrel(options =>
            {
                options.AddServerHeader = false;

                TryParse(configuration?["app:port"], out var port);
                options.Listen(IPAddress.Any, port);
            });
    }
}
