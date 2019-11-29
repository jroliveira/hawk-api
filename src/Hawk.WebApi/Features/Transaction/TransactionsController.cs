namespace Hawk.WebApi.Features.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;
    using Hawk.WebApi.Infrastructure.Pagination;

    using Microsoft.AspNetCore.Mvc;

    using static TransactionModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("transactions")]
    public class TransactionsController : BaseController
    {
        private readonly IGetTransactions getTransactions;
        private readonly IGetTransactionById getTransactionById;
        private readonly IUpsertTransaction upsertTransaction;
        private readonly IDeleteTransaction deleteTransaction;
        private readonly NewTransactionModelValidator validator;

        public TransactionsController(
            IGetTransactions getTransactions,
            IGetTransactionById getTransactionById,
            IUpsertTransaction upsertTransaction,
            IDeleteTransaction deleteTransaction)
        {
            this.getTransactions = getTransactions;
            this.getTransactionById = getTransactionById;
            this.upsertTransaction = upsertTransaction;
            this.deleteTransaction = deleteTransaction;
            this.validator = new NewTransactionModelValidator();
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TryModel<PageModel<TryModel<TransactionModel>>>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTransactions()
        {
            var entities = await this.getTransactions.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.Error<PageModel<TryModel<TransactionModel>>>,
                page => this.Ok(MapTransaction(page)));
        }

        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TryModel<TransactionModel>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTransactionById([FromRoute] string id)
        {
            var entity = await this.getTransactionById.GetResult(this.GetUser(), new Guid(id));

            return entity.Match(
                this.Error<TransactionModel>,
                transaction => this.Ok(new TryModel<TransactionModel>(new TransactionModel(transaction))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(TryModel<TransactionModel>), 201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTransaction([FromBody] NewTransactionModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<TransactionModel>(new InvalidObjectException("Invalid transaction.", validated));
            }

            var entity = await this.upsertTransaction.Execute(this.GetUser(), request);

            return entity.Match(
                this.Error<TransactionModel>,
                transaction => this.Created(transaction.Id, new TryModel<TransactionModel>(new TransactionModel(transaction))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TryModel<TransactionModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTransaction(
            [FromRoute] string id,
            [FromBody] NewTransactionModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<TransactionModel>(new InvalidObjectException("Invalid transaction.", validated));
            }

            var entity = await this.getTransactionById.GetResult(this.GetUser(), new Guid(id));

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertTransaction.Execute(this.GetUser(), NewTransactionModel.MapNewTransaction(new Guid(id), request));

                    return inserted.Match(
                        this.Error<TransactionModel>,
                        transaction => this.Created(new TryModel<TransactionModel>(new TransactionModel(transaction))));
                },
                async _ =>
                {
                    var updated = await this.upsertTransaction.Execute(this.GetUser(), request);

                    return updated.Match(
                        this.Error<TransactionModel>,
                        transaction => this.NoContent());
                });
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTransaction([FromRoute] string id)
        {
            var deleted = await this.deleteTransaction.Execute(this.GetUser(), new Guid(id));

            return deleted.Match(
                this.Error<TransactionModel>,
                _ => this.NoContent());
        }
    }
}
