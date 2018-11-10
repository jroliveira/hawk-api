namespace Hawk.Infrastructure.Data.Neo4J.Entities.Currency
{
    using System;

    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Currency.Currency;

    internal static class CurrencyMapping
    {
        private const string Name = "name";

        internal static Try<Currency> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => CreateWith(record.Get<string>(Name)),
            () => new NullReferenceException("Currency cannot be null."));
    }
}
