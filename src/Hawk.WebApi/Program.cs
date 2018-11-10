namespace Hawk.WebApi
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    using static System.IO.Directory;

    public sealed class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args)
            .Build()
            .Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => new WebHostBuilder()
            .UseKestrel(options => options.AddServerHeader = false)
            .UseContentRoot(GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) => config
                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .Build())
            .UseStartup<Startup>();
    }
}
