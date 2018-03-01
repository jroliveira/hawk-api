namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Hawk.Domain.Commands.Store;
    using Hawk.Domain.Queries.Store;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib;
    using Hawk.WebApi.Lib.Exceptions;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Lib.Validators;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    [Authorize]
    [ApiVersion("1")]
    [Route("stores")]
    public class StoresController : BaseController
    {
        private readonly PartialUpdater partialUpdater;
        private readonly IGetAllQuery getAll;
        private readonly IGetByNameQuery getByName;
        private readonly ICreateCommand create;
        private readonly IExcludeCommand exclude;
        private readonly StoreValidator validator;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public StoresController(
            IGetAllQuery getAll,
            IGetByNameQuery getByName,
            ICreateCommand create,
            IExcludeCommand exclude,
            IMapper mapper)
            : this(new PartialUpdater(), getAll, getByName, create, exclude, mapper, new StoreValidator())
        {
        }

        internal StoresController(
            PartialUpdater partialUpdater,
            IGetAllQuery getAll,
            IGetByNameQuery getByName,
            ICreateCommand create,
            IExcludeCommand exclude,
            IMapper mapper,
            StoreValidator validator)
        {
            this.partialUpdater = partialUpdater;
            this.getAll = getAll;
            this.getByName = getByName;
            this.create = create;
            this.exclude = exclude;
            this.mapper = mapper;
            this.validator = validator;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<Models.Store.Get.Store>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Models.Store.Get.Store>>(entities);

            return this.Ok(model);
        }

        /// <summary>
        /// Get by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(Models.Store.Get.Store), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var entity = await this.getByName.GetResult(name, this.User.GetClientId()).ConfigureAwait(false);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'stores' with name {name} could not be found");
            }

            var model = this.mapper.Map<Models.Store.Get.Store>(entity);

            return this.Ok(model);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Models.Store.Post.Store), 201)]
        public async Task<IActionResult> Create([FromBody] Models.Store.Post.Store request)
        {
            var validateResult = await this.validator.ValidateAsync(request).ConfigureAwait(false);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var entity = this.mapper.Map<Domain.Entities.Store>(request);
            var inserted = await this.create.Execute(entity, null).ConfigureAwait(false);
            var response = this.mapper.Map<Models.Store.Post.Store>(inserted);

            return this.Created(response.Name, response);
        }

        /// <summary>
        /// Update
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
            var entity = await this.getByName.GetResult(name, this.User.GetClientId()).ConfigureAwait(false);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'stores' with name {name} could not be found");
            }

            var model = this.mapper.Map<Models.Store.Get.Store>(entity);
            this.partialUpdater.Apply(request, model);
            var newEntity = this.mapper.Map<Domain.Entities.Store>(model);
            await this.create.Execute(newEntity, entity).ConfigureAwait(false);

            return this.NoContent();
        }

        /// <summary>
        /// Exclude
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Exclude([FromRoute] string name)
        {
            var entity = await this.getByName.GetResult(name, this.User.GetClientId()).ConfigureAwait(false);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'stores' with name {name} could not be found");
            }

            await this.exclude.Execute(entity).ConfigureAwait(false);

            return this.NoContent();
        }
    }
}