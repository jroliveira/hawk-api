namespace Hawk.WebApi.IntegrationTest.Features.Home
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Hawk.WebApi.IntegrationTest.Features.Shared;

    using Xunit;

    using static System.Net.HttpStatusCode;

    public sealed class HomeControllerTests : ControllerBaseTests
    {
        [Fact]
        public async Task GetHome_GivenUnauthorizedClient_WhenGetHome_ThenStatusCodeShouldBeOk()
        {
            var actual = await this.UnauthorizedClient.GetAsync("v1");

            actual.StatusCode.Should().Be(OK);
        }
    }
}
