﻿namespace Hawk.Infrastructure.Data.Neo4J.Entities.Currency
{
    using Hawk.Domain.Currency;
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Currency.Currency;

    internal static class CurrencyMapping
    {
        private const string Name = "name";

        internal static Try<Currency> MapFrom(Option<Record> record) => record.Match(
            some => CreateWith(some.Get<string>(Name)),
            () => new NotFoundException("Currency not found."));
    }
}
