namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Hawk.Domain.Queries.Tag;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Lib.Mappings;
    using Hawk.WebApi.Models;
    using Hawk.WebApi.Models.Tag.Get;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiVersion("1")]
    [Route("")]
    public class TagsController : BaseController
    {
        private readonly IGetAllQuery getAll;
        private readonly IGetAllByStoreQuery getAllByStore;

        public TagsController(
            IGetAllQuery getAll,
            IGetAllByStoreQuery getAllByStore)
        {
            this.getAll = getAll;
            this.getAllByStore = getAllByStore;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("tags")]
        [ProducesResponseType(typeof(Paged<Tag>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.GetUser(), this.Request.QueryString.Value).ConfigureAwait(false);

            return entities.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                paged => this.Ok(paged.ToModel()));
        }

        /// <summary>
        /// Get by store.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        [HttpGet("stores/{store}/tags")]
        [ProducesResponseType(typeof(Paged<Tag>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByStore(string store)
        {
            var entities = await this.getAllByStore.GetResult(this.GetUser(), store, this.Request.QueryString.Value).ConfigureAwait(false);

            return entities.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                paged => this.Ok(paged.ToModel()));
        }
    }
}