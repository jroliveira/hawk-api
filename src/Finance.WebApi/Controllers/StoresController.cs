namespace Finance.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Neo4j.Queries.Store;
    using Finance.WebApi.Models.Store.Get;

    using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAsync()
        {
            var entities = await this.getAll.GetResultAsync("junolive@gmail.com", this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Store>>(entities);

            return this.Ok(model);
        }
    }
}