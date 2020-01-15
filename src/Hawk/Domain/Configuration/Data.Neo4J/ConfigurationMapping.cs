namespace Hawk.Domain.Configuration.Data.Neo4J
{
    using System.Linq;

    using Hawk.Domain.Configuration;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Configuration.Configuration;
    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;
    using static Hawk.Domain.PaymentMethod.Data.Neo4J.PaymentMethodMapping;
    using static Hawk.Domain.Store.Data.Neo4J.StoreMapping;
    using static Hawk.Domain.Tag.Tag;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class ConfigurationMapping
    {
        private const string Currency = "currency";
        private const string Description = "description";
        private const string PaymentMethod = "paymentMethod";
        private const string Store = "store";
        private const string Tags = "tags";
        private const string Type = "type";

        internal static Try<Configuration> MapConfiguration(IRecord data) => MapRecord(data, "data").Match(
            record =>
            {
                var tags = record
                    .GetList(Tags)
                    .Select(tag => NewTag(tag))
                    .ToList();

                if (tags.Any(tag => !tag))
                {
                    return new InvalidObjectException("Invalid configuration.");
                }

                return NewConfiguration(
                    record.Get<string>(Type),
                    record.Get<string>(Description),
                    MapPaymentMethod(record.GetRecord(PaymentMethod)),
                    MapCurrency(record.GetRecord(Currency)),
                    MapStore(record.GetRecord(Store)),
                    Some(tags.Select(tag => tag.Get())));
            },
            () => new NotFoundException("Configuration not found."));
    }
}
