namespace Hawk.WebApi.IntegrationTest.Features.Shared
{
    using System;
    using System.Net.Http;

    using Hawk.WebApi.IntegrationTest.Infrastructure;
    using Hawk.WebApi.IntegrationTest.Infrastructure.Authentication;

    public abstract class ControllerBaseTests : IDisposable
    {
        protected ControllerBaseTests()
        {
            this.UnauthorizedClient = new TestServerFixture<Startup>().Client;
            this.AuthorizedClient = new TestServerFixture<AuthorizedStartup>().Client;
        }

        protected HttpClient AuthorizedClient { get; }

        protected HttpClient UnauthorizedClient { get; }

        public void Dispose()
        {
            this.AuthorizedClient?.Dispose();
            this.UnauthorizedClient?.Dispose();
        }
    }
}
