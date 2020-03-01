namespace Hawk.WebApi.Features.Configuration
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Configuration;

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

        public string Currency { get; }

        public string Payee { get; }

        public string Category { get; }

        public IEnumerable<string> Tags { get; }

        internal static ConfigurationModel NewConfigurationModel(Configuration entity) => new ConfigurationModel(entity);
    }
}
