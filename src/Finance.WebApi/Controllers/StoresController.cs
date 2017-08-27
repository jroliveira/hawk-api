namespace Finance.WebApi.Controllers
{
    using AutoMapper;

    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Neo4j.Queries.Store;
    using Finance.WebApi.Models.Store.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("stores")]
    public class StoresController : Controller
    {
        private readonly GetAllQuery getAll;
        private readonly IMapper mapper;

        public StoresController(
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
            var model = this.mapper.Map<Paged<Store>>(entities);

            return this.Ok(model);
        }
    }
}