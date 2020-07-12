namespace Hawk.Domain.Shared.Money.Data.Neo4J
{
    using System.Linq;

    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;
    using static Hawk.Domain.Shared.Money.Money;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class MoneyMapping
    {
        internal static Try<Money> MapMoney(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Money>(NotFound(nameof(Money))))(record => MapCurrency(record.GetRecord("currency"))
                .Select(currency => NewMoney(
                    record.Get<double>("value"),
                    currency)));
    }
}
