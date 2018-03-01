namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Hawk.Domain.Queries.PaymentMethod;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Models.PaymentMethod.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    [ApiVersion("1")]
    [Authorize]
    [Route("")]
    public class PaymentMethodsController : BaseController
    {
        private readonly IGetAllQuery getAll;
        private readonly IGetAllByStoreQuery getAllByStore;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public PaymentMethodsController(
            IGetAllQuery getAll,
            IGetAllByStoreQuery getAllByStore,
            IMapper mapper)
        {
            this.getAll = getAll;
            this.getAllByStore = getAllByStore;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        [HttpGet("payment-methods")]
        [ProducesResponseType(typeof(Paged<PaymentMethod>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<PaymentMethod>>(entities);

            return this.Ok(model);
        }

        /// <summary>
        /// Get by store
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        [HttpGet("stores/{store}/payment-methods")]
        [ProducesResponseType(typeof(PaymentMethod), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByStore(string store)
        {
            var entities = await this.getAllByStore.GetResult(this.User.GetClientId(), store, this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<PaymentMethod>>(entities);

            return this.Ok(model);
        }
    }
}