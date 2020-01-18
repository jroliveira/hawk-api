namespace Hawk.Domain.Account.Data.Neo4J
{
    using System;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Account.Account;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;

    internal static class AccountMapping
    {
        internal static Try<Account> MapAccount(IRecord data) => MapAccount(MapRecord(data, "data"));

        internal static Try<Account> MapAccount(Option<Neo4JRecord> record) => record.Match(
            some => NewAccount(some.Get<Guid>("id"), some.Get<string>("email")),
            () => new NotFoundException("Account not found."));
    }
}
