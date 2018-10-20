namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using System;

    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Entities.Payment.Price;

    internal static class PriceMapping
    {
        private const string Value = "value";
        private const string Currency = "currency";

        internal static Try<Price> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => CurrencyMapping.MapFrom(record.GetRecord(Currency)).Match(
                _ => _,
                currency => CreateWith(record.Get<double>(Value), currency)),
            () => new NullReferenceException("Price cannot be null."));
    }
}
