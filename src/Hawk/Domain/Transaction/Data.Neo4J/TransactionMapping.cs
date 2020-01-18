namespace Hawk.Domain.Transaction.Data.Neo4J
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver;

    using static Hawk.Domain.Store.Data.Neo4J.StoreMapping;
    using static Hawk.Domain.Tag.Data.Neo4J.TagMapping;
    using static Hawk.Domain.Transaction.Credit;
    using static Hawk.Domain.Transaction.Data.Neo4J.PaymentMapping;
    using static Hawk.Domain.Transaction.Debit;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class TransactionMapping
    {
        private static readonly IReadOnlyDictionary<string, Func<Option<Guid>, Option<Payment>, Option<Store>, Option<IEnumerable<Option<Tag>>>, Try<Transaction>>> Types =
            new Dictionary<string, Func<Option<Guid>, Option<Payment>, Option<Store>, Option<IEnumerable<Option<Tag>>>, Try<Transaction>>>
        {
            { "Debit", NewDebit },
            { "Credit", NewCredit },
        };

        internal static Try<Transaction> MapTransaction(IRecord data) => MapRecord(data, "data").Match(
            record =>
            {
                var type = record
                    .GetListOfString("type")
                    .Single(t => Types.ContainsKey(t.ToString()));

                if (!Types.TryGetValue(type, out var newTransaction))
                {
                    return new InvalidObjectException("Invalid transaction.");
                }

                return newTransaction(
                    record.Get<Guid>("id"),
                    MapPayment(record.GetRecord("payment")),
                    MapStore(record.GetRecord("store")),
                    Some(record.GetListOfNeo4JRecord("tags").Select(tag => MapTag(tag).ToOption())));
            },
            () => new NotFoundException("Transaction not found."));
    }
}
