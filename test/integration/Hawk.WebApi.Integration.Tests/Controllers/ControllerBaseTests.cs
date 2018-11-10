namespace Hawk.WebApi.Integration.Tests.Controllers
{
    using System;
    using System.Net.Http;

    using Hawk.WebApi.Integration.Tests.Lib;
    using Hawk.WebApi.Integration.Tests.Lib.Authentication;

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
