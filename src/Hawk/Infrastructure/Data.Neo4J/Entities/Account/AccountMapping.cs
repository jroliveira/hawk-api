namespace Hawk.Infrastructure.Data.Neo4J.Entities.Account
{
    using System;

    using Hawk.Domain.Account;
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.Account.Account;

    using static Neo4JRecord;

    internal static class AccountMapping
    {
        private const string Id = "id";
        private const string Email = "email";

        internal static Try<Account> MapAccount(IRecord data) => MapAccount(MapRecord(data, "data"));

        internal static Try<Account> MapAccount(Option<Neo4JRecord> record) => record.Match(
            some => NewAccount(some.Get<Guid>(Id), some.Get<string>(Email)),
            () => new NotFoundException("Account not found."));
    }
}
