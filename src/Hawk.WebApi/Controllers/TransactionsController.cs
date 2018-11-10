namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Lib.Mappings;
    using Hawk.WebApi.Lib.Validators;
    using Hawk.WebApi.Models;
    using Hawk.WebApi.Models.Transaction;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static System.Threading.Tasks.Util;

    [Authorize]
    [ApiVersion("1")]
    [Route("transactions")]
    public class TransactionsController : BaseController
    {
        private readonly IGetTransactions getTransactions;
        private readonly IGetTransactionById getTransactionById;
        private readonly IUpsertTransaction upsertTransaction;
        private readonly IDeleteTransaction deleteTransaction;
        private readonly TransactionValidator validator;

        public TransactionsController(
            IGetTransactions getTransactions,
            IGetTransactionById getTransactionById,
            IUpsertTransaction upsertTransaction,
            IDeleteTransaction deleteTransaction)
            : this(getTransactions, getTransactionById, upsertTransaction, deleteTransaction, new TransactionValidator())
        {
        }

        internal TransactionsController(
            IGetTransactions getTransactions,
            IGetTransactionById getTransactionById,
            IUpsertTransaction upsertTransaction,
            IDeleteTransaction deleteTransaction,
            TransactionValidator validator)
        {
            this.getTransactions = getTransactions;
            this.getTransactionById = getTransactionById;
            this.upsertTransaction = upsertTransaction;
            this.deleteTransaction = deleteTransaction;
            this.validator = validator;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<Models.Transaction.Get.Transaction>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getTransactions.GetResult(this.GetUser(), this.Request.QueryString.Value);

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
        [ProducesResponseType(typeof(Models.Transaction.Get.Transaction), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var entity = await this.getTransactionById.GetResult(id, this.GetUser());

            return entity.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                success => success.Match<IActionResult>(
                    transaction => this.Ok((Models.Transaction.Get.Transaction)transaction),
                    () => this.NotFound($"Resource 'transactions' with id {id} could not be found")));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Models.Transaction.Get.Transaction), 201)]
        public async Task<IActionResult> Create([FromBody] Models.Transaction.Post.Transaction request)
        {
            var validateResult = await this.validator.ValidateAsync(request);
            if (!validateResult.IsValid)
            {
                return this.StatusCode(409, validateResult.Errors);
            }

            request.Account = new Account(this.GetUser());
            var entity = await this.upsertTransaction.Execute(request);

            return entity.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                transaction => this.Created(transaction.Id, (Models.Transaction.Get.Transaction)transaction));
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
            var entity = await this.getTransactionById.GetResult(id, this.GetUser());

            return await entity.Match(
                failure => Task<IActionResult>(this.StatusCode(500, new Error(failure.Message))),
                success => success.Match<Task<IActionResult>>(
                    async transaction =>
                    {
                        Models.Transaction.Get.Transaction model = transaction;
                        PartialUpdater.Apply(request, model);
                        await this.upsertTransaction.Execute(model);

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
            var entity = await this.getTransactionById.GetResult(id, this.GetUser());

            return await entity.Match(
                failure => Task<IActionResult>(this.StatusCode(500, new Error(failure.Message))),
                success => success.Match<Task<IActionResult>>(
                    async store =>
                    {
                        await this.deleteTransaction.Execute(store);
                        return this.NoContent();
                    },
                    () => Task<IActionResult>(this.NotFound($"Resource 'transactions' with id {id} could not be found"))));
        }
    }
}
