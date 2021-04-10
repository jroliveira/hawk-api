namespace Hawk.Domain.Account.Data.Neo4J
{
    using System.Linq;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.Infrastructure.Monad.Utils;

    using static Hawk.Domain.Account.Money;
    using static Hawk.Domain.Currency.Currency;
    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;

    internal static class MoneyMapping
    {
        internal static Try<Money> MapMoney(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Util.Failure<Money>(Constants.ErrorMessages.NotFound(nameof(Money))))(record => NewMoney(
                record.Get<bool>("hidden"),
                MapCurrency(record.GetRecord("currency")).GetOrElse(DefaultCurrency)));
    }
}
