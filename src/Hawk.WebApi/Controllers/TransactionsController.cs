namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Transaction;
    using Hawk.Domain.Queries.Transaction;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Lib.Mappings;
    using Hawk.WebApi.Lib.Validators;
    using Hawk.WebApi.Models;
    using Hawk.WebApi.Models.Transaction;
    using Hawk.WebApi.Models.Transaction.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static System.Threading.Tasks.Util;

    [Authorize]
    [ApiVersion("1")]
    [Route("transactions")]
    public class TransactionsController : BaseController
    {
        private readonly IGetAllQuery getAll;
        private readonly IGetByIdQuery getById;
        private readonly ICreateCommand create;
        private readonly IExcludeCommand exclude;
        private readonly TransactionValidator validator;

        public TransactionsController(
            IGetAllQuery getAll,
            IGetByIdQuery getById,
            ICreateCommand create,
            IExcludeCommand exclude)
            : this(getAll, getById, create, exclude, new TransactionValidator())
        {
        }

        internal TransactionsController(
            IGetAllQuery getAll,
            IGetByIdQuery getById,
            ICreateCommand create,
            IExcludeCommand exclude,
            TransactionValidator validator)
        {
            this.getAll = getAll;
            this.getById = getById;
            this.create = create;
            this.exclude = exclude;
            this.validator = validator;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<Transaction>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                paged => this.Ok(paged.ToModel()));
        }

        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Transaction), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var entity = await this.getById.GetResult(id, this.GetUser());

            return entity.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                success => success.Match<IActionResult>(
                    transaction => this.Ok((Transaction)transaction),
                    () => this.NotFound($"Resource 'transactions' with id {id} could not be found")));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Transaction), 201)]
        public async Task<IActionResult> Create([FromBody] Models.Transaction.Post.Transaction request)
        {
            var validateResult = await this.validator.ValidateAsync(request);
            if (!validateResult.IsValid)
            {
                return this.StatusCode(409, validateResult.Errors);
            }

            request.Account = new Account(this.GetUser());
            var entity = await this.create.Execute(request);

            return entity.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                transaction => this.Created(transaction.Id, (Transaction)transaction));
        }

        /// <summary>
        /// Update.
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
            var entity = await this.getById.GetResult(id, this.GetUser());

            return await entity.Match(
                failure => Task<IActionResult>(this.StatusCode(500, new Error(failure.Message))),
                success => success.Match<Task<IActionResult>>(
                    async transaction =>
                    {
                        Transaction model = transaction;
                        PartialUpdater.Apply(request, model);
                        await this.create.Execute(model);

                        return this.NoContent();
                    },
                    () => Task<IActionResult>(this.NotFound($"Resource 'transactions' with id {id} could not be found"))));
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Exclude([FromRoute] string id)
        {
            var entity = await this.getById.GetResult(id, this.GetUser());

            return await entity.Match(
                failure => Task<IActionResult>(this.StatusCode(500, new Error(failure.Message))),
                success => success.Match<Task<IActionResult>>(
                    async store =>
                    {
                        await this.exclude.Execute(store);
                        return this.NoContent();
                    },
                    () => Task<IActionResult>(this.NotFound($"Resource 'transactions' with id {id} could not be found"))));
        }
    }
}
