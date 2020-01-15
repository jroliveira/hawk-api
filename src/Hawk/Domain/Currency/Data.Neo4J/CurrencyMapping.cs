namespace Hawk.Domain.Currency.Data.Neo4J
{
    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Currency.Currency;

    internal static class CurrencyMapping
    {
        private const string Name = "name";

        internal static Try<Currency> MapCurrency(Option<Neo4JRecord> record) => record.Match(
            some => NewCurrency(some.Get<string>(Name)),
            () => new NotFoundException("Currency not found."));
    }
}
