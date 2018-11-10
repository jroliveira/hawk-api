namespace Hawk.WebApi
{
    using Microsoft.AspNetCore.Hosting;

    using static System.IO.Directory;

    internal sealed class Program
    {
        internal static void Main(string[] args) => new WebHostBuilder()
            .UseKestrel(options => options.AddServerHeader = false)
            .UseContentRoot(GetCurrentDirectory())
            .UseStartup<Startup>()
            .Build()
            .Run();
    }
}
