namespace Hawk.WebApi.Models.Transaction.Get
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Lib.Mappings;

    public sealed class Transaction
    {
        public Transaction(string id, string type, Payment payment, Parcel parcel, string store, IEnumerable<string> tags, Account account)
        {
            this.Id = id;
            this.Type = type;
            this.Payment = payment;
            this.Parcel = parcel;
            this.Store = store;
            this.Tags = tags;
            this.Account = account;
        }

        public string Id { get; }

        public string Type { get; }

        public Payment Payment { get; }

        public Parcel Parcel { get; }

        public string Store { get; }

        public IEnumerable<string> Tags { get; }

        public Account Account { get; }

        public static implicit operator Transaction(Domain.Transaction.Transaction entity) => new Transaction(
            entity.Id.ToString(),
            entity.GetType().Name,
            entity.Payment,
            entity.Parcel,
            entity.Store,
            entity.Tags.Select(tag => tag.Name),
            entity.Account);

        public static implicit operator Option<Domain.Transaction.Transaction>(Transaction model) => model.ToEntity();

        public static implicit operator Domain.Transaction.Transaction(Transaction model) => model.ToEntity();
    }
}
