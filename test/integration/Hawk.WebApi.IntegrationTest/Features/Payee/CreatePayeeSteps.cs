namespace Hawk.WebApi.IntegrationTest.Features.Payee
{
    using System.Threading.Tasks;

    using TechTalk.SpecFlow;

    [Binding]
    public class CreatePayeeSteps
    {
        private readonly CreatePayeeDriver payeeDriver;

        public CreatePayeeSteps(CreatePayeeDriver payeeDriver) => this.payeeDriver = payeeDriver;

        [Given(@"a new payee")]
        public void GivenANewPayee()
        {
        }

        [When(@"I post payee new payee")]
        public Task WhenIPostPayeeData() => this.payeeDriver.PostNewPayee();

        [Then(@"a new payee should be created")]
        public void ThenANewPayeeShouldBeCreated() => this.payeeDriver.SuccessfullyCreated();
    }
}
