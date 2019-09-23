namespace Hawk.WebApi.Features.Transaction
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Infrastructure.Pagination;

    using static Infrastructure.ErrorHandling.ErrorHandler;

    public sealed class TransactionModel
    {
        public TransactionModel(Transaction entity)
            : this(
                entity.Id.ToString(),
                entity.GetType().Name,
                entity.Payment,
                entity.Store,
                entity.Tags.Select(tag => tag.Name))
        {
        }

        public TransactionModel(
            string id,
            string type,
            PaymentModel payment,
            string store,
            IEnumerable<string> tags)
        {
            this.Id = id;
            this.Type = type;
            this.Payment = payment;
            this.Store = store;
            this.Tags = tags;
        }

        public string Id { get; }

        public string Type { get; }

        public PaymentModel Payment { get; }

        public string Store { get; }

        public IEnumerable<string> Tags { get; }

        internal static TryModel<PageModel<TryModel<TransactionModel>>> MapFrom(Page<Try<Transaction>> @this) => new PageModel<TryModel<TransactionModel>>(
            @this
                .Data
                .Select(item => item.Match(
                    HandleError<TransactionModel>,
                    transaction => new TransactionModel(transaction))),
            @this.Skip,
            @this.Limit);
    }
}
