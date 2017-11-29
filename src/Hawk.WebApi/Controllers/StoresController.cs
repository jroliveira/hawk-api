namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Data.Neo4j.Queries.Store;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Models.Store.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("stores")]
    public class StoresController : BaseController
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
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Store>>(entities);

            return this.Ok(model);
        }
    }
}