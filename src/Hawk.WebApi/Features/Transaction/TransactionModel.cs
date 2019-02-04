namespace Hawk.WebApi.Features.Transaction
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using static Hawk.WebApi.Features.Shared.ErrorModels.GenericErrorModel;

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

        internal static Paged<object> MapFrom(Paged<Try<Transaction>> @this)
        {
            var model = @this
                .Data
                .Select(item => item.Match(
                    HandleError,
                    transaction => new TransactionModel(transaction)))
                .ToList();

            return new Paged<object>(model, @this.Skip, @this.Limit);
        }
    }
}
