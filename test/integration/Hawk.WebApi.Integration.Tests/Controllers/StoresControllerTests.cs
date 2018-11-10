namespace Hawk.WebApi.Integration.Tests.Controllers
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Hawk.WebApi.Integration.Tests.Lib;

    using Xunit;

    using static System.Net.HttpStatusCode;

    public sealed class StoresControllerTests : ControllerBaseTests
    {
        [Theory]
        [InlineData("v1/stores")]
        [InlineData("v1/stores/test")]
        public async Task Get_GivenUnauthorizedClient_WhenGetUrl_ThenStatusCodeShouldBeUnauthorized(string requestUri)
        {
            var testServer = new TestServerFixture<Startup>();

            var actual = await testServer.Client.GetAsync(requestUri);

            actual.StatusCode.Should().Be(Unauthorized);
        }

        [Fact]
        public async Task GetStores_GivenAuthorizedClient_WhenGetStores_ThenStatusCodeShouldBeOk()
        {
            var actual = await this.AuthorizedClient.GetAsync("v1/stores");

            actual.StatusCode.Should().Be(OK);
        }
    }
}
