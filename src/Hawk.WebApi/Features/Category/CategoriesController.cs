namespace Hawk.WebApi.Features.Category
{
    using System.Threading.Tasks;

    using Hawk.Domain.Category;
    using Hawk.Domain.Category.Commands;
    using Hawk.Domain.Category.Queries;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Domain.Shared.Commands.DeleteParam<string>;
    using static Hawk.Domain.Shared.Commands.UpsertParam<string, Hawk.Domain.Category.Category>;
    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Category.CategoryModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("categories")]
    public class CategoriesController : BaseController
    {
        private readonly IGetCategories getCategories;
        private readonly IGetCategoryByName getCategoryByName;
        private readonly IUpsertCategory upsertCategory;
        private readonly IDeleteCategory deleteCategory;
        private readonly CreateCategoryModelValidator validator;

        public CategoriesController(
            IGetCategories getCategories,
            IGetCategoryByName getCategoryByName,
            IUpsertCategory upsertCategory,
            IDeleteCategory deleteCategory)
        {
            this.getCategories = getCategories;
            this.getCategoryByName = getCategoryByName;
            this.upsertCategory = upsertCategory;
            this.deleteCategory = deleteCategory;
            this.validator = new CreateCategoryModelValidator();
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Try<Page<Try<CategoryModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategories()
        {
            var entities = await this.getCategories.GetResult(NewGetByAllParam(this.GetUser(), this.Request.QueryString.Value));

            return entities.Match(
                this.Error<Page<Try<CategoryModel>>>,
                page => this.Ok(page.ToPage(NewCategoryModel)));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(Try<CategoryModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategoryByName([FromRoute] string name)
        {
            var entity = await this.getCategoryByName.GetResult(NewGetByIdParam(this.GetUser(), name));

            return entity.Match(
                this.Error<CategoryModel>,
                category => this.Ok(Success(NewCategoryModel(category))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Try<CategoryModel>), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<CategoryModel>(new InvalidObjectException("Invalid category.", validated));
            }

            if (await this.getCategoryByName.GetResult(NewGetByIdParam(this.GetUser(), request.Name)))
            {
                return this.Error<CategoryModel>(new AlreadyExistsException("Category already exists."));
            }

            Option<Category> entity = request;
            var @try = await this.upsertCategory.Execute(NewUpsertParam(this.GetUser(), entity));

            return @try.Match(
                this.Error<CategoryModel>,
                _ => this.Created(entity.Get().Id, Success(NewCategoryModel(entity.Get()))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{name}")]
        [ProducesResponseType(typeof(Try<CategoryModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCategory(
            [FromRoute] string name,
            [FromBody] CreateCategoryModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<CategoryModel>(new InvalidObjectException("Invalid category.", validated));
            }

            var entity = await this.getCategoryByName.GetResult(NewGetByIdParam(this.GetUser(), name));

            Option<Category> newEntity = request;
            var @try = await this.upsertCategory.Execute(NewUpsertParam(this.GetUser(), name, newEntity));

            return @try.Match(
                this.Error<CategoryModel>,
                _ => entity
                    ? this.NoContent()
                    : this.Created(newEntity.Get().Id, Success(NewCategoryModel(newEntity.Get()))));
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCategory([FromRoute] string name)
        {
            var deleted = await this.deleteCategory.Execute(NewDeleteParam(this.GetUser(), name));

            return deleted.Match(
                this.Error<CategoryModel>,
                _ => this.NoContent());
        }
    }
}
