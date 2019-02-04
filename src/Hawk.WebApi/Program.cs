namespace Hawk.WebApi
{
    using Hawk.WebApi.Infrastructure.Api;
    using Hawk.WebApi.Infrastructure.Logging;
    using Hawk.WebApi.Infrastructure.Metric;

    using Microsoft.AspNetCore.Hosting;

    public sealed class Program
    {
        public static void Main(string[] args) => new WebHostBuilder()
            .ConfigureApi()
            .ConfigureLogging()
            .ConfigureMetric()
            .UseStartup<Startup>()
            .Build()
            .Run();
    }
}
