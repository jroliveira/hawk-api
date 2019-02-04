namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Currency;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Transaction.Price;

    internal static class PriceMapping
    {
        private const string Value = "value";
        private const string Currency = "currency";

        internal static Try<Price> MapFrom(Option<Record> record) => record.Match(
            some => CurrencyMapping.MapFrom(some.GetRecord(Currency)).Match(
                _ => _,
                currency => CreateWith(some.Get<double>(Value), currency)),
            () => new NotFoundException("Price not found."));
    }
}
