namespace Finance.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Finance.Entities.Transaction.Payment;
    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Neo4j.Queries.PaymentMethod;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PaymentMethodsController : Controller
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
        public async Task<IActionResult> GetAsync()
        {
            var entities = await this.getAll.GetResultAsync("junolive@gmail.com", this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Method>>(entities);

            return this.Ok(model);
        }

        [HttpGet("stores/{store}/payment-methods")]
        public async Task<IActionResult> GetByStoreSync(string store)
        {
            var entities = await this.getAllByStore.GetResultAsync("junolive@gmail.com", store, this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Method>>(entities);

            return this.Ok(model);
        }
    }
}