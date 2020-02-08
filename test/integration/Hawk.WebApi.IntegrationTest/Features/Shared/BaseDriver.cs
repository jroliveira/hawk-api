namespace Hawk.WebApi.IntegrationTest.Features.Shared
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;

    using static System.Threading.Tasks.Task;

    public abstract class BaseDriver : IDisposable
    {
        private readonly TestServer testServer;

        protected BaseDriver()
        {
            var builder = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) => config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true)
                    .AddEnvironmentVariables()
                    .Build())
                .UseStartup<Startup>();

            this.testServer = new TestServer(builder);
            this.HttpClient = this.testServer.CreateClient();
            this.Setup().GetAwaiter().GetResult();
        }

        protected HttpClient HttpClient { get; }

        public void Dispose()
        {
            this.testServer?.Dispose();
            this.HttpClient?.Dispose();
        }

        protected virtual Task Setup() => CompletedTask;
    }
}
