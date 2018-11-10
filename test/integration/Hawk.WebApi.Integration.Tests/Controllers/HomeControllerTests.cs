namespace Hawk.WebApi.Integration.Tests.Controllers
{
    using System.Threading.Tasks;

    using FluentAssertions;

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
