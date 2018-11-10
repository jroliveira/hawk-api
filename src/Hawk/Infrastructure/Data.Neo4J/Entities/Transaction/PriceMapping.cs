﻿namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Currency;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Transaction.Price;

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
