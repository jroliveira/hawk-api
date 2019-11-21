namespace Hawk.Infrastructure.Data.Neo4J.Entities.Configuration
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Currency;
    using Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Store;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.Configuration.Configuration;

    using static Neo4JRecord;

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
                    .Select(tag => Tag.NewTag(tag))
                    .ToList();

                if (tags.Any(tag => tag.IsFailure))
                {
                    return new InvalidObjectException("Invalid configuration.");
                }

                return NewConfiguration(
                    record.Get<string>(Type),
                    record.Get<string>(Description),
                    PaymentMethodMapping.MapPaymentMethod(record.GetRecord(PaymentMethod)),
                    CurrencyMapping.MapCurrency(record.GetRecord(Currency)),
                    StoreMapping.MapStore(record.GetRecord(Store)),
                    new List<Tag>(tags.Select(tag => tag.Get())));
            },
            () => new NotFoundException("Configuration not found."));
    }
}
