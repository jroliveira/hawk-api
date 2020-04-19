namespace Hawk.WebApi.Features.Transaction
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Domain.Transaction.Commands;
    using Hawk.Domain.Transaction.Queries;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Domain.Shared.Commands.DeleteParam<System.Guid>;
    using static Hawk.Domain.Shared.Commands.UpsertParam<System.Guid, Hawk.Domain.Transaction.Transaction>;
    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<System.Guid>;
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
        private readonly Func<Try<Email>, CreateTransactionModel, Task<ValidationResult>> validate;

        public TransactionsController(
            IGetTransactions getTransactions,
            IGetTransactionById getTransactionById,
            IUpsertTransaction upsertTransaction,
            IDeleteTransaction deleteTransaction,
            IGetCategoryByName getCategoryByName,
            IGetCurrencyByCode getCurrencyByCode,
            IGetPayeeByName getPayeeByName,
            IGetPaymentMethodByName getPaymentMethodByName)
        {
            this.getTransactions = getTransactions;
            this.getTransactionById = getTransactionById;
            this.upsertTransaction = upsertTransaction;
            this.deleteTransaction = deleteTransaction;

            this.validate = (email, request) =>
            {
                var validator = new CreateTransactionModelValidator(
                    email.Get(),
                    getCategoryByName,
                    getCurrencyByCode,
                    getPayeeByName,
                    getPaymentMethodByName,
                    getTransactions);

                return validator.ValidateAsync(request);
            };
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
            var entities = await this.getTransactions.GetResult(NewGetByAllParam(this.GetUser(), this.Request.QueryString.Value));

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
            var entity = await this.getTransactionById.GetResult(NewGetByIdParam(this.GetUser(), new Guid(id)));

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
            var validated = await this.validate(this.GetUser(), request);
            if (!validated.IsValid)
            {
                return this.Error<TransactionModel>(new InvalidObjectException("Invalid transaction.", validated));
            }

            Option<Transaction> entity = request;
            var @try = await this.upsertTransaction.Execute(NewUpsertParam(this.GetUser(), entity));

            return @try.Match(
                this.Error<TransactionModel>,
                _ => this.Created(entity.Get().Id, Success(NewTransactionModel(entity.Get()))));
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
            var validated = await this.validate(this.GetUser(), request);
            if (!validated.IsValid)
            {
                return this.Error<TransactionModel>(new InvalidObjectException("Invalid transaction.", validated));
            }

            var entity = await this.getTransactionById.GetResult(NewGetByIdParam(this.GetUser(), new Guid(id)));

            Option<Transaction> newEntity = request;
            var @try = await this.upsertTransaction.Execute(NewUpsertParam(this.GetUser(), new Guid(id), newEntity));

            return @try.Match(
                this.Error<TransactionModel>,
                _ => entity
                    ? this.NoContent()
                    : this.Created(newEntity.Get().Id, Success(NewTransactionModel(newEntity.Get()))));
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
            var deleted = await this.deleteTransaction.Execute(NewDeleteParam(this.GetUser(), new Guid(id)));

            return deleted.Match(
                this.Error<TransactionModel>,
                _ => this.NoContent());
        }
    }
}
