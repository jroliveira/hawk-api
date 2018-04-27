namespace Hawk.Infrastructure.Data.Neo4J.Mappings
{
    using System;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;
    using Hawk.Infrastructure.Monad;
    using Neo4j.Driver.V1;
    using static Hawk.Domain.Entities.Account;

    internal static class AccountMapping
    {
        private const string Data = "data";
        private const string Id = "id";
        private const string Email = "email";

        public static Try<Account> MapFrom(IRecord data) => MapFrom(data.GetRecord(Data));

        public static Try<Account> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => CreateWith(record.Get<Guid>(Id), record.Get<string>(Email)),
            () => new NullReferenceException("Account cannot be null."));
    }
}
