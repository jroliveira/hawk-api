namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Lib.Mappings;
    using Hawk.WebApi.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Tag = Hawk.WebApi.Models.Tag.Get.Tag;

    [Authorize]
    [ApiVersion("1")]
    [Route("")]
    public class TagsController : BaseController
    {
        private readonly IGetTags getTags;
        private readonly IGetTagsByStore getTagsByStore;

        public TagsController(
            IGetTags getTags,
            IGetTagsByStore getTagsByStore)
        {
            this.getTags = getTags;
            this.getTagsByStore = getTagsByStore;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("tags")]
        [ProducesResponseType(typeof(Paged<Tag>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getTags.GetResult(this.GetUser(), this.Request.QueryString.Value);

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
            var entities = await this.getTagsByStore.GetResult(this.GetUser(), store, this.Request.QueryString.Value);

            return entities.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                paged => this.Ok(paged.ToModel()));
        }
    }
}
