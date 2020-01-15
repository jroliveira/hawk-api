namespace Hawk.WebApi.Features.Configuration
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Configuration;

    public sealed class ConfigurationModel
    {
        public ConfigurationModel(Configuration entity)
            : this(
                entity.Type,
                entity.Description,
                entity.PaymentMethod,
                entity.Currency,
                entity.Store,
                entity.Tags.Select(tag => tag.Value))
        {
        }

        public ConfigurationModel(
            string type,
            string description,
            string paymentMethod,
            string currency,
            string store,
            IEnumerable<string> tags)
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

        public string PaymentMethod { get; }

        public string Currency { get; }

        public string Store { get; }

        public IEnumerable<string> Tags { get; }
    }
}
