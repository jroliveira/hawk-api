namespace Hawk.WebApi.Features.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Transaction;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure;
    using Hawk.WebApi.Infrastructure.Authentication;
    using Hawk.WebApi.Infrastructure.ErrorHandling;
    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Infrastructure.Pagination;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using static TransactionModel;

    [Authorize]
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
            IDeleteTransaction deleteTransaction,
            IHostingEnvironment environment)
            : this(getTransactions, getTransactionById, upsertTransaction, deleteTransaction, new NewTransactionModelValidator(), environment)
        {
        }

        internal TransactionsController(
            IGetTransactions getTransactions,
            IGetTransactionById getTransactionById,
            IUpsertTransaction upsertTransaction,
            IDeleteTransaction deleteTransaction,
            NewTransactionModelValidator validator,
            IHostingEnvironment environment)
            : base(environment)
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
        [ProducesResponseType(typeof(TryModel<PageModel<TryModel<TransactionModel>>>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTransactions()
        {
            var entities = await this.getTransactions.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.HandleError<PageModel<TryModel<TransactionModel>>>,
                page => this.Ok(MapFrom(page)));
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
                this.HandleError<TransactionModel>,
                transaction => this.Ok(new TryModel<TransactionModel>(new TransactionModel(transaction))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateSchema(typeof(NewTransactionModel))]
        [ProducesResponseType(typeof(TryModel<TransactionModel>), 201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTransaction([FromBody] NewTransactionModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.HandleError<TransactionModel>(new InvalidObjectException("Invalid transaction.", validated));
            }

            var entity = await this.upsertTransaction.Execute(this.GetUser(), request);

            return entity.Match(
                this.HandleError<TransactionModel>,
                transaction => this.Created(transaction.Id, new TryModel<TransactionModel>(new TransactionModel(transaction))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ValidateSchema(typeof(NewTransactionModel))]
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
                return this.HandleError<TransactionModel>(new InvalidObjectException("Invalid transaction.", validated));
            }

            var entity = await this.getTransactionById.GetResult(this.GetUser(), new Guid(id));

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertTransaction.Execute(this.GetUser(), NewTransactionModel.MapFrom(new Guid(id), request));

                    return inserted.Match(
                        this.HandleError<TransactionModel>,
                        transaction => this.Created(default, new TryModel<TransactionModel>(new TransactionModel(transaction))));
                },
                async _ =>
                {
                    var updated = await this.upsertTransaction.Execute(this.GetUser(), request);

                    return updated.Match(
                        this.HandleError<TransactionModel>,
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
                this.HandleError<TransactionModel>,
                _ => this.NoContent());
        }
    }
}
