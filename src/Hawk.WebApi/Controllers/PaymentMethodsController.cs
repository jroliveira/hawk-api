namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Hawk.Entities.Transaction.Payment;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Data.Neo4j.Queries.PaymentMethod;
    using Hawk.WebApi.Lib.Extensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PaymentMethodsController : BaseController
    {
        private readonly GetAllQuery getAll;
        private readonly GetAllByStoreQuery getAllByStore;
        private readonly IMapper mapper;

        public PaymentMethodsController(
            GetAllQuery getAll,
            GetAllByStoreQuery getAllByStore,
            IMapper mapper)
        {
            this.getAll = getAll;
            this.getAllByStore = getAllByStore;
            this.mapper = mapper;
        }

        [HttpGet("payment-methods")]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Method>>(entities);

            return this.Ok(model);
        }

        [HttpGet("stores/{store}/payment-methods")]
        public async Task<IActionResult> GetByStore(string store)
        {
            var entities = await this.getAllByStore.GetResult(this.User.GetClientId(), store, this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Method>>(entities);

            return this.Ok(model);
        }
    }
}