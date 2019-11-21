namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Transaction.Price;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Currency.CurrencyMapping;

    internal static class PriceMapping
    {
        private const string Value = "value";
        private const string Currency = "currency";

        internal static Try<Price> MapPrice(Option<Neo4JRecord> record) => record.Match(
            some => MapCurrency(some.GetRecord(Currency)).Match(
                _ => _,
                currency => NewPrice(some.Get<double>(Value), currency)),
            () => new NotFoundException("Price not found."));
    }
}
