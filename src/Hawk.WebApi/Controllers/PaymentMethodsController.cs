namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Hawk.Domain.Queries.PaymentMethod;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Lib.Mappings;
    using Hawk.WebApi.Models;
    using Hawk.WebApi.Models.PaymentMethod.Get;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion("1")]
    [Authorize]
    [Route("")]
    public class PaymentMethodsController : BaseController
    {
        private readonly IGetAllQuery getAll;
        private readonly IGetAllByStoreQuery getAllByStore;

        public PaymentMethodsController(
            IGetAllQuery getAll,
            IGetAllByStoreQuery getAllByStore)
        {
            this.getAll = getAll;
            this.getAllByStore = getAllByStore;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("payment-methods")]
        [ProducesResponseType(typeof(Paged<PaymentMethod>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.GetUser(), this.Request.QueryString.Value).ConfigureAwait(false);

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
        [ProducesResponseType(typeof(PaymentMethod), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByStore(string store)
        {
            var entities = await this.getAllByStore.GetResult(this.GetUser(), store, this.Request.QueryString.Value).ConfigureAwait(false);

            return entities.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                paged => this.Ok(paged.ToModel()));
        }
    }
}