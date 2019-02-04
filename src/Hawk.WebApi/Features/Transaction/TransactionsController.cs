namespace Hawk.WebApi.Features.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(typeof(Paged<TransactionModel>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getTransactions.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.HandleError,
                paged => this.Ok(TransactionModel.MapFrom(paged)));
        }

        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TransactionModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var entity = await this.getTransactionById.GetResult(this.GetUser(), new Guid(id));

            return entity.Match(
                this.HandleError,
                transaction => this.Ok(new TransactionModel(transaction)));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateSchema(typeof(NewTransactionModel))]
        [ProducesResponseType(typeof(TransactionModel), 201)]
        public async Task<IActionResult> Create([FromBody] NewTransactionModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.HandleError(new InvalidObjectException("Invalid transaction.", validated));
            }

            var entity = await this.upsertTransaction.Execute(this.GetUser(), request);

            return entity.Match(
                this.HandleError,
                transaction => this.Created(transaction.Id, new TransactionModel(transaction)));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ValidateSchema(typeof(NewTransactionModel))]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update(
            [FromRoute] string id,
            [FromBody] NewTransactionModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.HandleError(new InvalidObjectException("Invalid transaction.", validated));
            }

            var entity = await this.getTransactionById.GetResult(this.GetUser(), new Guid(id));

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertTransaction.Execute(this.GetUser(), NewTransactionModel.MapFrom(new Guid(id), request));

                    return inserted.Match(
                        this.HandleError,
                        transaction => this.Created(default, new TransactionModel(transaction)));
                },
                async _ =>
                {
                    var updated = await this.upsertTransaction.Execute(this.GetUser(), request);

                    return updated.Match(
                        this.HandleError,
                        configuration => this.NoContent());
                });
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
            var deleted = await this.deleteTransaction.Execute(this.GetUser(), new Guid(id));

            return deleted.Match(
                this.HandleError,
                configuration => this.NoContent());
        }
    }
}
