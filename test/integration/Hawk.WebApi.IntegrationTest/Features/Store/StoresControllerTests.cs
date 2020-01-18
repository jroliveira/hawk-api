namespace Hawk.WebApi.IntegrationTest.Features.Payee
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Hawk.WebApi.IntegrationTest.Features.Shared;
    using Hawk.WebApi.IntegrationTest.Infrastructure;

    using Xunit;
    using Xunit.Abstractions;

    using static System.Net.HttpStatusCode;

    public sealed class PayeesControllerTests : ControllerBaseTests
    {
        private readonly ITestOutputHelper output;

        public PayeesControllerTests(ITestOutputHelper output) => this.output = output;

        [Theory]
        [InlineData("v1/payees")]
        [InlineData("v1/payees/test")]
        public async Task Get_GivenUnauthorizedClient_WhenGetUrl_ThenStatusCodeShouldBeUnauthorized(string requestUri)
        {
            var testServer = new TestServerFixture<Startup>();

            var actual = await testServer.Client.GetAsync(requestUri);
            this.output.WriteLine(await actual.Content.ReadAsStringAsync());

            actual.StatusCode.Should().Be(Unauthorized);
        }

        [Fact]
        public async Task GetPayees_GivenAuthorizedClient_WhenGetPayees_ThenStatusCodeShouldBeOk()
        {
            var actual = await this.AuthorizedClient.GetAsync("v1/payees");
            this.output.WriteLine(await actual.Content.ReadAsStringAsync());

            actual.StatusCode.Should().Be(OK);
        }
    }
}
