namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Hawk.Domain.Queries.Tag;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    [Authorize]
    [ApiVersion("1")]
    [Route("")]
    public class TagsController : BaseController
    {
        private readonly IGetAllQuery getAll;
        private readonly IGetAllByStoreQuery getAllByStore;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public TagsController(
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
        [HttpGet("tags")]
        [ProducesResponseType(typeof(Paged<Tag>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Tag>>(entities);

            return this.Ok(model);
        }

        /// <summary>
        /// Get by store
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        [HttpGet("stores/{store}/tags")]
        [ProducesResponseType(typeof(Tag), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByStore(string store)
        {
            var entities = await this.getAllByStore.GetResult(this.User.GetClientId(), store, this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Tag>>(entities);

            return this.Ok(model);
        }
    }
}