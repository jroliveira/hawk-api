namespace Hawk.WebApi.Features.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Tag;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;
    using Hawk.WebApi.Infrastructure.ErrorHandling;
    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Infrastructure.Pagination;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using static TagModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("")]
    public class TagsController : BaseController
    {
        private readonly IGetTags getTags;
        private readonly IGetTagsByStore getTagsByStore;

        public TagsController(
            IGetTags getTags,
            IGetTagsByStore getTagsByStore,
            IWebHostEnvironment environment)
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
        [ProducesResponseType(typeof(TryModel<PageModel<TryModel<TagModel>>>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTags()
        {
            var entities = await this.getTags.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.HandleError<PageModel<TryModel<TagModel>>>,
                page => this.Ok(MapTag(page)));
        }

        /// <summary>
        /// Get by store.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        [HttpGet("stores/{store}/tags")]
        [ProducesResponseType(typeof(TryModel<PageModel<TryModel<TagModel>>>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTagsByStore(string store)
        {
            var entities = await this.getTagsByStore.GetResult(this.GetUser(), store, this.Request.QueryString.Value);

            return entities.Match(
                this.HandleError<PageModel<TryModel<TagModel>>>,
                page => this.Ok(MapTag(page)));
        }
    }
}
