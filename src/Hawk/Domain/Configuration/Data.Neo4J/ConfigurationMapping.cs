namespace Hawk.Domain.Configuration.Data.Neo4J
{
    using System.Linq;

    using Hawk.Domain.Configuration;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver;

    using static Hawk.Domain.Configuration.Configuration;
    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;
    using static Hawk.Domain.Payee.Data.Neo4J.PayeeMapping;
    using static Hawk.Domain.PaymentMethod.Data.Neo4J.PaymentMethodMapping;
    using static Hawk.Domain.Tag.Data.Neo4J.TagMapping;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class ConfigurationMapping
    {
        internal static Try<Configuration> MapConfiguration(IRecord data) => MapRecord(data, "data").Match(
            record => NewConfiguration(
                record.Get<string>("type"),
                record.Get<string>("description"),
                MapPaymentMethod(record.GetRecord("paymentMethod")),
                MapCurrency(record.GetRecord("currency")),
                MapPayee(record.GetRecord("payee")),
                Some(record.GetListOfNeo4JRecord("tags").Select(tag => MapTag(tag).ToOption()))),
            () => new NotFoundException("Configuration not found."));
    }
}
