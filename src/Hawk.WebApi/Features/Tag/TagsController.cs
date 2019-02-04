namespace Hawk.WebApi.Features.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using static TagModel;

    [Authorize]
    [ApiVersion("1")]
    [Route("")]
    public class TagsController : BaseController
    {
        private readonly IGetTags getTags;
        private readonly IGetTagsByStore getTagsByStore;

        public TagsController(
            IGetTags getTags,
            IGetTagsByStore getTagsByStore,
            IHostingEnvironment environment)
            : base(environment)
        {
            this.getTags = getTags;
            this.getTagsByStore = getTagsByStore;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("tags")]
        [ProducesResponseType(typeof(Paged<TagModel>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getTags.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.HandleError,
                paged => this.Ok(MapFrom(paged)));
        }

        /// <summary>
        /// Get by store.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        [HttpGet("stores/{store}/tags")]
        [ProducesResponseType(typeof(Paged<TagModel>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByStore(string store)
        {
            var entities = await this.getTagsByStore.GetResult(this.GetUser(), store, this.Request.QueryString.Value);

            return entities.Match(
                this.HandleError,
                paged => this.Ok(MapFrom(paged)));
        }
    }
}
