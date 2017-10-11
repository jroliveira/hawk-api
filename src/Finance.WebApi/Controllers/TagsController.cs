namespace Finance.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Neo4j.Queries.Tag;
    using Finance.WebApi.Models.Tag.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class TagsController : Controller
    {
        private readonly GetAllQuery getAll;
        private readonly GetAllByStoreQuery getAllByStore;
        private readonly IMapper mapper;

        public TagsController(
            GetAllQuery getAll,
            GetAllByStoreQuery getAllByStore,
            IMapper mapper)
        {
            this.getAll = getAll;
            this.getAllByStore = getAllByStore;
            this.mapper = mapper;
        }

        [HttpGet("tags")]
        public async Task<IActionResult> GetAsync()
        {
            var entities = await this.getAll.GetResultAsync("junolive@gmail.com", this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Tag>>(entities);

            return this.Ok(model);
        }

        [HttpGet("stores/{store}/tags")]
        public async Task<IActionResult> GetByStoreAsync(string store)
        {
            var entities = await this.getAllByStore.GetResultAsync("junolive@gmail.com", store, this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Tag>>(entities);

            return this.Ok(model);
        }
    }
}