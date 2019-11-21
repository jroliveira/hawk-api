namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.Tag.Tag;
    using static Hawk.Domain.Transaction.Credit;
    using static Hawk.Domain.Transaction.Debit;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Store.StoreMapping;

    using static Neo4JRecord;
    using static PaymentMapping;

    internal static class TransactionMapping
    {
        private const string Type = "type";
        private const string Payment = "payment";
        private const string Id = "id";
        private const string Tags = "tags";
        private const string Store = "store";

        private static readonly IReadOnlyDictionary<string, Func<Option<Guid>, Option<Payment>, Option<Store>, Option<IReadOnlyCollection<Tag>>, Try<Transaction>>> Types =
            new Dictionary<string, Func<Option<Guid>, Option<Payment>, Option<Store>, Option<IReadOnlyCollection<Tag>>, Try<Transaction>>>
        {
            { "Debit", NewDebit },
            { "Credit", NewCredit },
        };

        internal static Try<Transaction> MapTransaction(IRecord data) => MapRecord(data, "data").Match(
            record =>
            {
                var type = record
                    .GetList(Type)
                    .Single(t => Types.ContainsKey(t.ToString()));

                if (!Types.TryGetValue(type, out var newTransaction))
                {
                    return new InvalidObjectException("Invalid transaction.");
                }

                var tags = record
                    .GetList(Tags)
                    .Select(tag => NewTag(tag))
                    .ToList();

                if (tags.Any(tag => tag.IsFailure))
                {
                    return new InvalidObjectException("Invalid transaction.");
                }

                return newTransaction(
                    record.Get<Guid>(Id),
                    MapPayment(record.GetRecord(Payment)),
                    MapStore(record.GetRecord(Store)),
                    new List<Tag>(tags.Select(tag => tag.Get())));
            },
            () => new NotFoundException("Transaction not found."));
    }
}
