namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Hawk.Domain.Commands.Transaction;
    using Hawk.Domain.Exceptions;
    using Hawk.Domain.Queries.Transaction;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib;
    using Hawk.WebApi.Lib.Exceptions;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Lib.Validators;
    using Hawk.WebApi.Models.Transaction;
    using Hawk.WebApi.Models.Transaction.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    [Authorize]
    [ApiVersion("1")]
    [Route("transactions")]
    public class TransactionsController : BaseController
    {
        private readonly PartialUpdater partialUpdater;
        private readonly IGetAllQuery getAll;
        private readonly IGetByIdQuery getById;
        private readonly ICreateCommand create;
        private readonly IExcludeCommand exclude;
        private readonly TransactionValidator validator;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public TransactionsController(
            IGetAllQuery getAll,
            IGetByIdQuery getById,
            ICreateCommand create,
            IExcludeCommand exclude,
            IMapper mapper)
            : this(new PartialUpdater(), getAll, getById, create, exclude, mapper, new TransactionValidator())
        {
        }

        internal TransactionsController(
            PartialUpdater partialUpdater,
            IGetAllQuery getAll,
            IGetByIdQuery getById,
            ICreateCommand create,
            IExcludeCommand exclude,
            IMapper mapper,
            TransactionValidator validator)
        {
            this.partialUpdater = partialUpdater;
            this.getAll = getAll;
            this.getById = getById;
            this.create = create;
            this.exclude = exclude;
            this.validator = validator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Transaction), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var entity = await this.getById.GetResult(id, this.User.GetClientId()).ConfigureAwait(false);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'transactions' with id {id} could not be found");
            }

            var model = this.mapper.Map<Transaction>(entity);

            return this.Ok(model);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<Transaction>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Transaction>>(entities);

            return this.Ok(model);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Transaction), 201)]
        public async Task<IActionResult> Create([FromBody] Models.Transaction.Post.Transaction request)
        {
            var validateResult = await this.validator.ValidateAsync(request).ConfigureAwait(false);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            request.Account = new Account(this.User.GetClientId());
            var entity = this.mapper.Map<Domain.Entities.Transaction>(request);
            var inserted = await this.create.Execute(entity).ConfigureAwait(false);
            var response = this.mapper.Map<Transaction>(inserted);

            return this.Created(response.Id, response);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update(
            [FromRoute] string id,
            [FromBody] dynamic request)
        {
            var entity = await this.getById.GetResult(id, this.User.GetClientId()).ConfigureAwait(false);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'transactions' with id {id} could not be found");
            }

            var model = this.mapper.Map<Transaction>(entity);
            this.partialUpdater.Apply(request, model);
            entity = this.mapper.Map<Domain.Entities.Transaction>(model);
            await this.create.Execute(entity).ConfigureAwait(false);

            return this.NoContent();
        }

        /// <summary>
        /// Exclude
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Exclude([FromRoute] string id)
        {
            var entity = await this.getById.GetResult(id, this.User.GetClientId()).ConfigureAwait(false);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'transactions' with id {id} could not be found");
            }

            await this.exclude.Execute(entity).ConfigureAwait(false);

            return this.NoContent();
        }
    }
}