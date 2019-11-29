namespace Hawk.WebApi.Features.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;
    using Hawk.WebApi.Infrastructure.Pagination;

    using Microsoft.AspNetCore.Mvc;

    using static PaymentMethodModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("")]
    public class PaymentMethodsController : BaseController
    {
        private readonly IGetPaymentMethods getPaymentMethods;
        private readonly IGetPaymentMethodsByStore getPaymentMethodsByStore;

        public PaymentMethodsController(
            IGetPaymentMethods getPaymentMethods,
            IGetPaymentMethodsByStore getPaymentMethodsByStore)
        {
            this.getPaymentMethods = getPaymentMethods;
            this.getPaymentMethodsByStore = getPaymentMethodsByStore;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("payment-methods")]
        [ProducesResponseType(typeof(TryModel<PageModel<TryModel<PaymentMethodModel>>>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var entities = await this.getPaymentMethods.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.Error<PageModel<TryModel<PaymentMethodModel>>>,
                page => this.Ok(MapPaymentMethod(page)));
        }

        /// <summary>
        /// Get by store.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        [HttpGet("stores/{store}/payment-methods")]
        [ProducesResponseType(typeof(TryModel<PageModel<TryModel<PaymentMethodModel>>>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentMethodsByStore(string store)
        {
            var entities = await this.getPaymentMethodsByStore.GetResult(this.GetUser(), store, this.Request.QueryString.Value);

            return entities.Match(
                this.Error<PageModel<TryModel<PaymentMethodModel>>>,
                page => this.Ok(MapPaymentMethod(page)));
        }
    }
}
