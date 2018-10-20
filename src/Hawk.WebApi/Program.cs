namespace Hawk.WebApi
{
    using System.IO;

    using Microsoft.AspNetCore.Hosting;

    internal sealed class Program
    {
        public static void Main(string[] args) => new WebHostBuilder()
            .UseKestrel(options => options.AddServerHeader = false)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .Build()
            .Run();
    }
}
