namespace Hawk.WebApi.Features.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Tag.TagModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("")]
    public class TagsController : BaseController
    {
        private readonly IGetTags getTags;
        private readonly IGetTagsByPayee getTagsByPayee;
        private readonly IGetTagByName getTagByName;
        private readonly IUpsertTag upsertTag;
        private readonly IDeleteTag deleteTag;
        private readonly CreateTagModelValidator validator;

        public TagsController(
            IGetTags getTags,
            IGetTagsByPayee getTagsByPayee,
            IGetTagByName getTagByName,
            IUpsertTag upsertTag,
            IDeleteTag deleteTag)
        {
            this.getTags = getTags;
            this.getTagsByPayee = getTagsByPayee;
            this.getTagByName = getTagByName;
            this.upsertTag = upsertTag;
            this.deleteTag = deleteTag;
            this.validator = new CreateTagModelValidator();
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("tags")]
        [ProducesResponseType(typeof(Try<Page<Try<TagModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTags()
        {
            var entities = await this.getTags.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.Error<Page<Try<TagModel>>>,
                page => this.Ok(page.ToPage(NewTagModel)));
        }

        /// <summary>
        /// Get by payee.
        /// </summary>
        /// <param name="payee"></param>
        /// <returns></returns>
        [HttpGet("payees/{payee}/tags")]
        [ProducesResponseType(typeof(Try<Page<Try<TagModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTagsByPayee(string payee)
        {
            var entities = await this.getTagsByPayee.GetResult(this.GetUser(), payee, this.Request.QueryString.Value);

            return entities.Match(
                this.Error<Page<Try<TagModel>>>,
                page => this.Ok(page.ToPage(NewTagModel)));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("tags/{name}")]
        [ProducesResponseType(typeof(Try<TagModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTagByName([FromRoute] string name)
        {
            var entity = await this.getTagByName.GetResult(this.GetUser(), name);

            return entity.Match(
                this.Error<TagModel>,
                tag => this.Ok(Success(NewTagModel(tag))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("tags")]
        [ProducesResponseType(typeof(Try<TagModel>), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagModel request)
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
                        tag => this.Created(tag.Value, Success(NewTagModel(tag))));
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
        [ProducesResponseType(typeof(Try<TagModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTag(
            [FromRoute] string name,
            [FromBody] CreateTagModel request)
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
                        tag => this.Created(Success(NewTagModel(tag))));
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
