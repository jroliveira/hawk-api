namespace Hawk.WebApi.Features.Transaction
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Transaction;

    public sealed class TransactionModel
    {
        private TransactionModel(Transaction entity)
        {
            this.Id = entity.Id.ToString();
            this.Type = entity.Type;
            this.Payment = entity.Payment;
            this.Store = entity.Store;
            this.Tags = entity.Tags.Select(tag => tag.Value);
        }

        public string Id { get; }

        public string Type { get; }

        public PaymentModel Payment { get; }

        public string Store { get; }

        public IEnumerable<string> Tags { get; }

        internal static TransactionModel NewTransactionModel(Transaction entity) => new TransactionModel(entity);
    }
}
