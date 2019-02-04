namespace Hawk.Infrastructure.Data.Neo4J.Entities.Account
{
    using System;

    using Hawk.Domain.Account;
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.Account.Account;

    internal static class AccountMapping
    {
        private const string Id = "id";
        private const string Email = "email";
        private const string Data = "data";

        internal static Try<Account> MapFrom(IRecord data) => MapFrom(data.GetRecord(Data));

        internal static Try<Account> MapFrom(Option<Record> record) => record.Match(
            some => CreateWith(some.Get<Guid>(Id), some.Get<string>(Email)),
            () => new NotFoundException("Account not found."));
    }
}
