namespace Hawk.WebApi.Features.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    using static TagModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("")]
    public class TagsController : BaseController
    {
        private readonly IGetTags getTags;
        private readonly IGetTagsByStore getTagsByStore;
        private readonly IGetTagByName getTagByName;
        private readonly IUpsertTag upsertTag;
        private readonly IDeleteTag deleteTag;
        private readonly NewTagModelValidator validator;

        public TagsController(
            IGetTags getTags,
            IGetTagsByStore getTagsByStore,
            IGetTagByName getTagByName,
            IUpsertTag upsertTag,
            IDeleteTag deleteTag)
        {
            this.getTags = getTags;
            this.getTagsByStore = getTagsByStore;
            this.getTagByName = getTagByName;
            this.upsertTag = upsertTag;
            this.deleteTag = deleteTag;
            this.validator = new NewTagModelValidator();
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("tags")]
        [ProducesResponseType(typeof(TryModel<Page<TryModel<TagModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTags()
        {
            var entities = await this.getTags.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.Error<Page<TryModel<TagModel>>>,
                page => this.Ok(MapTag(page)));
        }

        /// <summary>
        /// Get by store.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        [HttpGet("stores/{store}/tags")]
        [ProducesResponseType(typeof(TryModel<Page<TryModel<TagModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTagsByStore(string store)
        {
            var entities = await this.getTagsByStore.GetResult(this.GetUser(), store, this.Request.QueryString.Value);

            return entities.Match(
                this.Error<Page<TryModel<TagModel>>>,
                page => this.Ok(MapTag(page)));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("tags/{name}")]
        [ProducesResponseType(typeof(TryModel<TagModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTagByName([FromRoute] string name)
        {
            var entity = await this.getTagByName.GetResult(this.GetUser(), name);

            return entity.Match(
                this.Error<TagModel>,
                tag => this.Ok(new TryModel<TagModel>(new TagModel(tag))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("tags")]
        [ProducesResponseType(typeof(TryModel<TagModel>), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTag([FromBody] NewTagModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<TagModel>(new InvalidObjectException("Invalid tag.", validated));
            }

            var entity = await this.getTagByName.GetResult(this.GetUser(), request.Name);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertTag.Execute(this.GetUser(), request.Name, request);

                    return inserted.Match(
                        this.Error<TagModel>,
                        tag => this.Created(tag.Value, new TryModel<TagModel>(new TagModel(tag))));
                },
                _ => Task(this.Error<TagModel>(new AlreadyExistsException("Tag already exists."))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("tags/{name}")]
        [ProducesResponseType(typeof(TryModel<TagModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTag(
            [FromRoute] string name,
            [FromBody] NewTagModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<TagModel>(new InvalidObjectException("Invalid tag.", validated));
            }

            var entity = await this.getTagByName.GetResult(this.GetUser(), name);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertTag.Execute(this.GetUser(), name, request);

                    return inserted.Match(
                        this.Error<TagModel>,
                        tag => this.Created(new TryModel<TagModel>(new TagModel(tag))));
                },
                async _ =>
                {
                    var updated = await this.upsertTag.Execute(this.GetUser(), name, request);

                    return updated.Match(
                        this.Error<TagModel>,
                        tag => this.NoContent());
                });
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("tags/{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTag([FromRoute] string name)
        {
            var deleted = await this.deleteTag.Execute(this.GetUser(), name);

            return deleted.Match(
                this.Error<TagModel>,
                _ => this.NoContent());
        }
    }
}
