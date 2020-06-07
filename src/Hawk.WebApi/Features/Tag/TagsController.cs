namespace Hawk.WebApi.Features.Tag
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Tag.Commands;
    using Hawk.Domain.Tag.Queries;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Domain.Shared.Commands.DeleteParam<string>;
    using static Hawk.Domain.Shared.Commands.UpsertParam<string, Hawk.Domain.Tag.Tag>;
    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;
    using static Hawk.Domain.Tag.Queries.GetTagsByPayeeParam;
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
        private readonly IGetPayeeByName getPayeeByName;
        private readonly Func<Try<Email>, string, CreateTagModel, Task<ValidationResult>> validate;

        public TagsController(
            IGetTags getTags,
            IGetTagsByPayee getTagsByPayee,
            IGetTagByName getTagByName,
            IUpsertTag upsertTag,
            IDeleteTag deleteTag,
            IGetPayeeByName getPayeeByName,
            Func<Try<Email>, string, CreateTagModel, Task<ValidationResult>> validate)
        {
            this.getTags = getTags;
            this.getTagsByPayee = getTagsByPayee;
            this.getTagByName = getTagByName;
            this.upsertTag = upsertTag;
            this.deleteTag = deleteTag;
            this.getPayeeByName = getPayeeByName;
            this.validate = validate;
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
            var entities = await this.getTags.GetResult(NewGetByAllParam(this.GetUser(), this.Request.QueryString.Value));

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
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTagsByPayee(string payee)
        {
            var payeeEntity = await this.getPayeeByName.GetResult(NewGetByIdParam(this.GetUser(), payee));
            if (!payeeEntity)
            {
                return this.Error<TagModel>(new NotFoundException("Payee not found."));
            }

            var entities = await this.getTagsByPayee.GetResult(NewGetTagsByPayeeParam(this.GetUser(), payeeEntity, this.Request.QueryString.Value));

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
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTagByName([FromRoute] string name)
        {
            var entity = await this.getTagByName.GetResult(NewGetByIdParam(this.GetUser(), name));

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
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagModel request)
        {
            var validated = await this.validate(this.GetUser(), request.Name, request);
            if (!validated.IsValid)
            {
                return this.Error<TagModel>(new InvalidObjectException("Invalid tag.", validated));
            }

            if (await this.getTagByName.GetResult(NewGetByIdParam(this.GetUser(), request.Name)))
            {
                return this.Error<TagModel>(new AlreadyExistsException("Tag already exists."));
            }

            Option<Tag> entity = request;
            var @try = await this.upsertTag.Execute(NewUpsertParam(this.GetUser(), entity));

            return @try.Match(
                this.Error<TagModel>,
                _ => this.Created(entity.Get().Id, Success(NewTagModel(entity.Get()))));
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
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTag(
            [FromRoute] string name,
            [FromBody] CreateTagModel request)
        {
            var validated = await this.validate(this.GetUser(), name, request);
            if (!validated.IsValid)
            {
                return this.Error<TagModel>(new InvalidObjectException("Invalid tag.", validated));
            }

            if (await this.getTagByName.GetResult(NewGetByIdParam(this.GetUser(), request.Name)))
            {
                return this.Error<TagModel>(new AlreadyExistsException("Tag already exists."));
            }

            var entity = await this.getTagByName.GetResult(NewGetByIdParam(this.GetUser(), name));
            Option<Tag> newEntity = request;
            var @try = await this.upsertTag.Execute(NewUpsertParam(this.GetUser(), name, request));

            return @try.Match(
                this.Error<TagModel>,
                _ => entity
                    ? this.NoContent()
                    : this.Created(newEntity.Get().Id, Success(NewTagModel(newEntity.Get()))));
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("tags/{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTag([FromRoute] string name)
        {
            var deleted = await this.deleteTag.Execute(NewDeleteParam(this.GetUser(), name));

            return deleted.Match(
                this.Error<TagModel>,
                _ => this.NoContent());
        }
    }
}
