namespace Hawk.Domain.Configuration
{
    using System.Collections.Generic;

    using Hawk.Domain.Currency;
    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Configuration
    {
        private Configuration(
            string type,
            string description,
            PaymentMethod paymentMethod,
            Currency currency,
            Store store,
            IReadOnlyCollection<Tag> tags)
        {
            this.Type = type;
            this.Description = description;
            this.PaymentMethod = paymentMethod;
            this.Currency = currency;
            this.Store = store;
            this.Tags = tags;
        }

        public string Type { get; }

        public string Description { get; }

        public PaymentMethod PaymentMethod { get; }

        public Currency Currency { get; }

        public Store Store { get; }

        public IReadOnlyCollection<Tag> Tags { get; }

        public static Try<Configuration> NewConfiguration(
            Option<string> type,
            Option<string> description,
            Option<PaymentMethod> paymentMethod,
            Option<Currency> currency,
            Option<Store> store,
            Option<IReadOnlyCollection<Tag>> tags) =>
                type
                && description
                && paymentMethod
                && currency
                && store
                && tags
                ? new Configuration(type.Get(), description.Get(), paymentMethod.Get(), currency.Get(), store.Get(), tags.Get())
                : Failure<Configuration>(new InvalidObjectException("Invalid configuration."));
    }
}
