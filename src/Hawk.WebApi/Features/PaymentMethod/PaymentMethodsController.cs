namespace Hawk.WebApi.Features.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.WebApi.Features.PaymentMethod.PaymentMethodModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("")]
    public class PaymentMethodsController : BaseController
    {
        private readonly IGetPaymentMethods getPaymentMethods;
        private readonly IGetPaymentMethodsByPayee getPaymentMethodsByPayee;

        public PaymentMethodsController(
            IGetPaymentMethods getPaymentMethods,
            IGetPaymentMethodsByPayee getPaymentMethodsByPayee)
        {
            this.getPaymentMethods = getPaymentMethods;
            this.getPaymentMethodsByPayee = getPaymentMethodsByPayee;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("payment-methods")]
        [ProducesResponseType(typeof(Try<Page<Try<PaymentMethodModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var entities = await this.getPaymentMethods.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.Error<Page<Try<PaymentMethodModel>>>,
                page => this.Ok(page.ToPage(NewPaymentMethodModel)));
        }

        /// <summary>
        /// Get by payee.
        /// </summary>
        /// <param name="payee"></param>
        /// <returns></returns>
        [HttpGet("payees/{payee}/payment-methods")]
        [ProducesResponseType(typeof(Try<Page<Try<PaymentMethodModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentMethodsByPayee(string payee)
        {
            var entities = await this.getPaymentMethodsByPayee.GetResult(this.GetUser(), payee, this.Request.QueryString.Value);

            return entities.Match(
                this.Error<Page<Try<PaymentMethodModel>>>,
                page => this.Ok(page.ToPage(NewPaymentMethodModel)));
        }
    }
}
