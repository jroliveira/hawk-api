namespace Hawk.Domain.Currency.Data.Neo4J
{
    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Currency.Currency;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;

    internal static class CurrencyMapping
    {
        private const string Name = "name";
        private const string Total = "total";

        internal static Try<(Currency Currency, uint Count)> MapCurrency(IRecord data) => MapRecord(data, "data").Match(
            record =>
            {
                var total = record.Get<uint>(Total);
                if (!total.IsDefined)
                {
                    return new InvalidObjectException("Invalid currency.");
                }

                return MapCurrency(record).Match<Try<(Currency, uint)>>(
                    _ => _,
                    currency => (currency, total.Get()));
            },
            () => new NotFoundException("Currency not found."));

        internal static Try<Currency> MapCurrency(Option<Neo4JRecord> record) => record.Match(
            some => NewCurrency(some.Get<string>(Name)),
            () => new NotFoundException("Currency not found."));
    }
}
