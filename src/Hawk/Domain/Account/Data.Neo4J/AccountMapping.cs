namespace Hawk.Domain.Account.Data.Neo4J
{
    using System;
    using System.Linq;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Account.Account;
    using static Hawk.Domain.Shared.Email;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class AccountMapping
    {
        internal static Try<Account> MapAccount(in IRecord data) => MapAccount(MapRecord(data, "data"));

        internal static Try<Account> MapAccount(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Account>(NotFound(nameof(Account))))(record => NewAccount(
                record.Get<Guid>("id"),
                NewEmail(record.Get<string>("email"))));
    }
}
