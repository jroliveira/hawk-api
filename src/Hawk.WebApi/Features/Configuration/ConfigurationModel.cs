namespace Hawk.WebApi.Features.Configuration
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Configuration;
    using Hawk.WebApi.Features.Payee;
    using Hawk.WebApi.Features.Shared.Money;

    public sealed class ConfigurationModel
    {
        private ConfigurationModel(Configuration entity)
        {
            this.Type = entity.Type;
            this.Description = entity.Id;
            this.PaymentMethod = entity.PaymentMethod;
            this.Currency = entity.Currency;
            this.Payee = entity.Payee;
            this.Category = entity.Category;
            this.Tags = entity.Tags.Select(tag => tag.Id);
        }

        public string Type { get; }

        public string Description { get; }

        public string PaymentMethod { get; }

        public CurrencyModel Currency { get; }

        public PayeeModel Payee { get; }

        public string Category { get; }

        public IEnumerable<string> Tags { get; }

        internal static ConfigurationModel NewConfigurationModel(in Configuration entity) => new ConfigurationModel(entity);
    }
}
