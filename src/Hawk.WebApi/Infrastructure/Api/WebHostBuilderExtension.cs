namespace Hawk.WebApi.Infrastructure.Api
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    using static System.IO.Directory;

    internal static class WebHostBuilderExtension
    {
        internal static IWebHostBuilder ConfigureApi(this IWebHostBuilder @this) => @this
            .UseKestrel(options => options.AddServerHeader = false)
            .UseContentRoot(GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) => config
                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .Build())
            .UseUrls("http://*:5000");
    }
}
