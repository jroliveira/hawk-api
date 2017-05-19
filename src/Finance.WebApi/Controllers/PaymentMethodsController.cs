namespace Finance.WebApi.Controllers
{
    using AutoMapper;

    using Finance.Entities.Transaction.Payment;
    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Neo4j.Queries.PaymentMethod;

    using Microsoft.AspNetCore.Mvc;

    [Route("/payment-methods")]
    public class PaymentMethodsController : Controller
    {
        private readonly GetAllQuery getAll;
        private readonly IMapper mapper;

        public PaymentMethodsController(
            GetAllQuery getAll,
            IMapper mapper)
        {
            this.getAll = getAll;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var entities = this.getAll.GetResult("junolive@gmail.com", this.Request.QueryString.Value);
            var model = this.mapper.Map<Paged<Method>>(entities);

            return this.Ok(model);
        }
    }
}