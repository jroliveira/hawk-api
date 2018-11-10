namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Account;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Account;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Store;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver.V1;

    internal static class TransactionMapping
    {
        private const string Type = "type";
        private const string Account = "account";
        private const string Payment = "payment";
        private const string Id = "id";
        private const string Tags = "tags";
        private const string Parcel = "parcel";
        private const string Store = "store";
        private const string Data = "data";

        private static readonly IReadOnlyDictionary<string, Func<Option<Guid>, Option<Payment>, Option<Account>, Try<Transaction>>> Types =
            new Dictionary<string, Func<Option<Guid>, Option<Payment>, Option<Account>, Try<Transaction>>>
        {
            { "Debit", Debit.CreateWith },
            { "Credit", Credit.CreateWith },
        };

        internal static Try<Transaction> MapFrom(IRecord data) => data.GetRecord(Data).Match(
            record =>
            {
                var type = record
                    .GetList(Type)
                    .Single(t => Types.ContainsKey(t.ToString()));

                if (!Types.TryGetValue(type, out var createWith))
                {
                    return new KeyNotFoundException($"Transaction's type {type} not configured.");
                }

                var account = AccountMapping.MapFrom(record.GetRecord(Account));
                var payment = PaymentMapping.MapFrom(record.GetRecord(Payment));

                return createWith(record.Get<Guid>(Id), payment, account).Select(transaction =>
                {
                    transaction.AddTags(record.GetList(Tags).Select(tag => Tag.CreateWith(tag).ToOption()).ToList());
                    transaction.SplittedIn(ParcelMapping.MapFrom(record.GetRecord(Parcel)));
                    transaction.UpdateStore(StoreMapping.MapFrom(record.GetRecord(Store)).GetOrElse((default, default)).Store);

                    return transaction;
                });
            },
            () => new NullReferenceException("Transaction cannot be null."));
    }
}
