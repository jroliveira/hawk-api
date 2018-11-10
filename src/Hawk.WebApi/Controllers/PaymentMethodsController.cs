namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Lib.Mappings;
    using Hawk.WebApi.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion("1")]
    [Authorize]
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
        [ProducesResponseType(typeof(Paged<Models.PaymentMethod.Get.PaymentMethod>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getPaymentMethods.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                paged => this.Ok(paged.ToModel()));
        }

        /// <summary>
        /// Get by store.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        [HttpGet("stores/{store}/payment-methods")]
        [ProducesResponseType(typeof(Models.PaymentMethod.Get.PaymentMethod), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByStore(string store)
        {
            var entities = await this.getPaymentMethodsByStore.GetResult(this.GetUser(), store, this.Request.QueryString.Value);

            return entities.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                paged => this.Ok(paged.ToModel()));
        }
    }
}
