namespace Hawk.WebApi.Features.Transaction
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.String;

    public sealed class TransactionModel
    {
        private TransactionModel(Transaction entity)
        {
            this.Id = entity.Id.ToString();
            this.Type = entity.Type.ToString();
            this.Status = entity.Status.ToString();
            this.Description = entity.Description.GetOrElse(Empty);
            this.Payment = entity.Payment;
            this.Payee = entity.Payee;
            this.Category = entity.Category;
            this.Tags = entity.Tags.Select(tag => tag.Id);
        }

        public string Id { get; }

        public string Type { get; }

        public string Status { get; }

        public string Description { get; }

        public PaymentModel Payment { get; }

        public string Payee { get; }

        public string Category { get; }

        public IEnumerable<string> Tags { get; }

        internal static TransactionModel NewTransactionModel(Transaction entity) => new TransactionModel(entity);
    }
}
