﻿namespace Hawk.WebApi.Features.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Transaction.TransactionModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("transactions")]
    public class TransactionsController : BaseController
    {
        private readonly IGetTransactions getTransactions;
        private readonly IGetTransactionById getTransactionById;
        private readonly IUpsertTransaction upsertTransaction;
        private readonly IDeleteTransaction deleteTransaction;
        private readonly CreateTransactionModelValidator validator;

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
            this.validator = new CreateTransactionModelValidator();
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Try<Page<Try<TransactionModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTransactions()
        {
            var entities = await this.getTransactions.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.Error<Page<Try<TransactionModel>>>,
                page => this.Ok(page.ToPage(NewTransactionModel)));
        }

        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Try<TransactionModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTransactionById([FromRoute] string id)
        {
            var entity = await this.getTransactionById.GetResult(this.GetUser(), new Guid(id));

            return entity.Match(
                this.Error<TransactionModel>,
                transaction => this.Ok(Success(NewTransactionModel(transaction))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Try<TransactionModel>), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<TransactionModel>(new InvalidObjectException("Invalid transaction.", validated));
            }

            var entity = await this.upsertTransaction.Execute(this.GetUser(), request);

            return entity.Match(
                this.Error<TransactionModel>,
                transaction => this.Created(transaction.Id, Success(NewTransactionModel(transaction))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Try<TransactionModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTransaction(
            [FromRoute] string id,
            [FromBody] CreateTransactionModel request)
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
                    var inserted = await this.upsertTransaction.Execute(this.GetUser(), request);

                    return inserted.Match(
                        this.Error<TransactionModel>,
                        transaction => this.Created(Success(NewTransactionModel(transaction))));
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
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
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
