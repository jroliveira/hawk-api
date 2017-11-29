namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Data.Neo4j.Queries.Tag;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Models.Tag.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class TagsController : BaseController
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
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Tag>>(entities);

            return this.Ok(model);
        }

        [HttpGet("stores/{store}/tags")]
        public async Task<IActionResult> GetByStore(string store)
        {
            var entities = await this.getAllByStore.GetResult(this.User.GetClientId(), store, this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Tag>>(entities);

            return this.Ok(model);
        }
    }
}