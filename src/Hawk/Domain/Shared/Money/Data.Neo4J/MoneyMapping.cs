namespace Hawk.Domain.Shared.Money.Data.Neo4J
{
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;
    using static Hawk.Domain.Shared.Money.Money;

    internal static class MoneyMapping
    {
        internal static Try<Money> MapMoney(Option<Neo4JRecord> record) => record.Match(
            some => MapCurrency(some.GetRecord("currency")).Match(
                _ => _,
                currency => NewMoney(some.Get<double>("value"), currency)),
            () => new NotFoundException("Money not found."));
    }
}
