namespace Hawk.WebApi.IntegrationTest.Features.Store
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Hawk.WebApi.IntegrationTest.Features.Shared;
    using Hawk.WebApi.IntegrationTest.Infrastructure;

    using Xunit;
    using Xunit.Abstractions;

    using static System.Net.HttpStatusCode;

    public sealed class StoresControllerTests : ControllerBaseTests
    {
        private readonly ITestOutputHelper output;

        public StoresControllerTests(ITestOutputHelper output) => this.output = output;

        [Theory]
        [InlineData("v1/stores")]
        [InlineData("v1/stores/test")]
        public async Task Get_GivenUnauthorizedClient_WhenGetUrl_ThenStatusCodeShouldBeUnauthorized(string requestUri)
        {
            var testServer = new TestServerFixture<Startup>();

            var actual = await testServer.Client.GetAsync(requestUri);
            this.output.WriteLine(await actual.Content.ReadAsStringAsync());

            actual.StatusCode.Should().Be(Unauthorized);
        }

        [Fact]
        public async Task GetStores_GivenAuthorizedClient_WhenGetStores_ThenStatusCodeShouldBeOk()
        {
            var actual = await this.AuthorizedClient.GetAsync("v1/stores");
            this.output.WriteLine(await actual.Content.ReadAsStringAsync());

            actual.StatusCode.Should().Be(OK);
        }
    }
}
