namespace Finance.WebApi.Controllers
{
    using AutoMapper;

    using Finance.Entities.Transaction.Payment;
    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Neo4j.Queries.PaymentMethod;

    using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Get()
        {
            var entities = this.getAll.GetResult("junolive@gmail.com", this.Request.QueryString.Value);
            var model = this.mapper.Map<Paged<Method>>(entities);

            return this.Ok(model);
        }

        [HttpGet("stores/{store}/payment-methods")]
        public IActionResult GetByStore(string store)
        {
            var entities = this.getAllByStore.GetResult("junolive@gmail.com", store, this.Request.QueryString.Value);
            var model = this.mapper.Map<Paged<Method>>(entities);

            return this.Ok(model);
        }
    }
}