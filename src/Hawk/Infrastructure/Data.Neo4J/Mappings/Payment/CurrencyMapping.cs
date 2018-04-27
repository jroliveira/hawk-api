namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using System;
    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure.Monad;
    using static Hawk.Domain.Entities.Payment.Currency;

    internal static class CurrencyMapping
    {
        private const string Name = "name";

        public static Try<Currency> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => CreateWith(record.Get<string>(Name)),
            () => new NullReferenceException("Currency cannot be null."));
    }
}