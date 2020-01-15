namespace Hawk.Domain.Transaction.Data.Neo4J
{
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;
    using static Hawk.Domain.Transaction.Price;

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
