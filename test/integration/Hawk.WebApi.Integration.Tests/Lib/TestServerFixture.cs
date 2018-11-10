﻿namespace Hawk.WebApi.Integration.Tests.Lib
{
    using System;
    using System.Net.Http;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;

    using static ProjectPathFinder;

    public class TestServerFixture<TStartup> : IDisposable
        where TStartup : Startup
    {
        private static readonly string ContentRoot = GetPath("src", typeof(TStartup));
        private readonly TestServer testServer;

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(ContentRoot)
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<TStartup>();

            this.testServer = new TestServer(builder);
            this.Client = this.testServer.CreateClient();
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            this.testServer?.Dispose();
            this.Client?.Dispose();
        }
    }
}
