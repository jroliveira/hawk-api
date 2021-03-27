namespace Hawk.WebApi.IntegrationTest.Features.Payee
{
    using System.Net;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Hawk.WebApi.Features.Payee;
    using Hawk.WebApi.IntegrationTest.Features.Shared;

    using static System.Net.HttpStatusCode;

    using static Hawk.WebApi.IntegrationTest.Infrastructure.HttpClientExtension;

    public sealed class CreatePayeeDriver : BaseDriver
    {
        private readonly CreatePayeeModel model = new CreatePayeeModel("Test", default);
        private (HttpStatusCode StatusCode, dynamic? Response) response;

        public async Task PostNewPayee() => this.response = await this.HttpClient.Post("v1/payees", this.model);

        public void SuccessfullyCreated() => this.response.StatusCode.Should().Be(Created);

        protected override Task Setup() => this.HttpClient.Delete($"v1/payees/{this.model.Name}");
    }
}
