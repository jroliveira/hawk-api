namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Store;
    using Hawk.Domain.Queries.Store;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Lib.Mappings;
    using Hawk.WebApi.Lib.Validators;
    using Hawk.WebApi.Models;
    using Hawk.WebApi.Models.Store.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static System.Threading.Tasks.Util;

    [Authorize]
    [ApiVersion("1")]
    [Route("stores")]
    public class StoresController : BaseController
    {
        private readonly IGetAllQuery getAll;
        private readonly IGetByNameQuery getByName;
        private readonly ICreateCommand create;
        private readonly IExcludeCommand exclude;
        private readonly StoreValidator validator;

        public StoresController(
            IGetAllQuery getAll,
            IGetByNameQuery getByName,
            ICreateCommand create,
            IExcludeCommand exclude)
            : this(getAll, getByName, create, exclude, new StoreValidator())
        {
        }

        internal StoresController(
            IGetAllQuery getAll,
            IGetByNameQuery getByName,
            ICreateCommand create,
            IExcludeCommand exclude,
            StoreValidator validator)
        {
            this.getAll = getAll;
            this.getByName = getByName;
            this.create = create;
            this.exclude = exclude;
            this.validator = validator;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<Store>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                paged => this.Ok(paged.ToModel()));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(Store), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var entity = await this.getByName.GetResult(name, this.GetUser());

            return entity.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                success => success.Match<IActionResult>(
                    store => this.Ok(new Store(store)),
                    () => this.NotFound($"Resource 'stores' with name {name} could not be found")));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Models.Store.Post.Store), 201)]
        public async Task<IActionResult> Create([FromBody] Models.Store.Post.Store request)
        {
            var validateResult = await this.validator.ValidateAsync(request);
            if (!validateResult.IsValid)
            {
                return this.StatusCode(409, validateResult.Errors);
            }

            var entity = await this.create.Execute(request);

            return entity.Match(
                failure => this.StatusCode(500, failure.Message),
                store => this.Created(store.Name, new Store(store.Name)));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{name}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update(
            [FromRoute] string name,
            [FromBody] dynamic request)
        {
            var entity = await this.getByName.GetResult(name, this.GetUser());

            return await entity.Match(
                failure => Task<IActionResult>(this.StatusCode(500, new Error(failure.Message))),
                success => success.Match<Task<IActionResult>>(
                    async store =>
                    {
                        Store model = store;
                        PartialUpdater.Apply(request, model);
                        await this.create.Execute(model);

                        return this.NoContent();
                    },
                    () => Task<IActionResult>(this.NotFound($"Resource 'stores' with name {name} could not be found"))));
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Exclude([FromRoute] string name)
        {
            var entity = await this.getByName.GetResult(name, this.GetUser());

            return await entity.Match(
                failure => Task<IActionResult>(this.StatusCode(500, new Error(failure.Message))),
                success => success.Match<Task<IActionResult>>(
                    async store =>
                    {
                        await this.exclude.Execute(store);
                        return this.NoContent();
                    },
                    () => Task<IActionResult>(this.NotFound($"Resource 'stores' with name {name} could not be found"))));
        }
    }
}
