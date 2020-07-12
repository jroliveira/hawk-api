namespace Hawk.Domain.Currency.Data.Neo4J
{
    using System.Linq;

    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Currency.Currency;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class CurrencyMapping
    {
        internal static Try<Currency> MapCurrency(in IRecord data) => MapCurrency(MapRecord(data, "data"));

        internal static Try<Currency> MapCurrency(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Currency>(NotFound(nameof(Currency))))(record => NewCurrency(
                record.Get<string>("code"),
                record.Get<string>("symbol"),
                record.Get<uint>("transactions")));
    }
}
