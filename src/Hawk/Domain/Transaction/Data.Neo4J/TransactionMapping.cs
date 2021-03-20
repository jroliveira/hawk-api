namespace Hawk.Domain.Transaction.Data.Neo4J
{
    using System;
    using System.Linq;

    using Hawk.Domain.Shared.Transaction;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver;

    using static Hawk.Domain.Category.Data.Neo4J.CategoryMapping;
    using static Hawk.Domain.Payee.Data.Neo4J.PayeeMapping;
    using static Hawk.Domain.Tag.Data.Neo4J.TagMapping;
    using static Hawk.Domain.Transaction.Data.Neo4J.PaymentMapping;
    using static Hawk.Domain.Transaction.Transaction;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class TransactionMapping
    {
        internal static Try<Transaction> MapTransaction(in IRecord data) => MapRecord(data, "data")
            .Fold(Failure<Transaction>(NotFound(nameof(Transaction))))(record => NewTransaction(
                record.Get<Guid>("id"),
                record
                    .GetListOfString("type")
                    .Select(type => type.ToEnum<TransactionType>())
                    .Single(type => type),
                record
                    .Get<string>("status")
                    .Select(some => some.ToEnum<TransactionStatus>()),
                record.Get<string>("description"),
                MapPayment(record.GetRecord("payment")),
                MapPayee(record.GetRecord("payee")),
                MapCategory(record.GetRecord("category")),
                Some(record.GetListOfNeo4JRecord("tags").Select(tag => MapTag(tag).ToOption()))));
    }
}
