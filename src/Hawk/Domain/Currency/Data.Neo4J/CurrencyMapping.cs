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
        internal static Try<Currency> MapCurrency(IRecord data) => MapCurrency(MapRecord(data, "data"));

        internal static Try<Currency> MapCurrency(Option<Neo4JRecord> record) => record.Match(
            some => NewCurrency(some.Get<string>("name"), some.Get<uint>("transactions")),
            () => new NotFoundException("Currency not found."));
    }
}
