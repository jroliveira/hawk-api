namespace Hawk.WebApi.Features.Category
{
    using System.Threading.Tasks;

    using Hawk.Domain.Category;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

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
            var entities = await this.getCategories.GetResult(this.GetUser(), this.Request.QueryString.Value);

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
            var entity = await this.getCategoryByName.GetResult(this.GetUser(), name);

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

            var entity = await this.getCategoryByName.GetResult(this.GetUser(), request.Name);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertCategory.Execute(this.GetUser(), request.Name, request);

                    return inserted.Match(
                        this.Error<CategoryModel>,
                        category => this.Created(category.Value, Success(NewCategoryModel(category))));
                },
                _ => Task(this.Error<CategoryModel>(new AlreadyExistsException("Category already exists."))));
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

            var entity = await this.getCategoryByName.GetResult(this.GetUser(), name);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertCategory.Execute(this.GetUser(), name, request);

                    return inserted.Match(
                        this.Error<CategoryModel>,
                        category => this.Created(Success(NewCategoryModel(category))));
                },
                async _ =>
                {
                    var updated = await this.upsertCategory.Execute(this.GetUser(), name, request);

                    return updated.Match(
                        this.Error<CategoryModel>,
                        category => this.NoContent());
                });
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
            var deleted = await this.deleteCategory.Execute(this.GetUser(), name);

            return deleted.Match(
                this.Error<CategoryModel>,
                _ => this.NoContent());
        }
    }
}
