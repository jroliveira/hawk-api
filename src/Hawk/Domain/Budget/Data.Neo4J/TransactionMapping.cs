namespace Hawk.Domain.Budget.Data.Neo4J
{
    using System;
    using System.Linq;

    using Hawk.Domain.Shared.Transaction;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Budget.Transaction;
    using static Hawk.Domain.Shared.Money.Data.Neo4J.MoneyMapping;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class TransactionMapping
    {
        internal static Try<Transaction> MapTransaction(Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Transaction>(NotFound(nameof(Transaction))))(record => NewTransaction(
                record.Get<Guid>("id"),
                record
                    .GetListOfString("type")
                    .Select(type => type.ToEnum<TransactionType>())
                    .Single(type => type),
                Date(
                    record.Get<int>("year"),
                    record.Get<int>("month"),
                    record.Get<int>("day")),
                MapMoney(record.GetRecord("cost"))));
    }
}
