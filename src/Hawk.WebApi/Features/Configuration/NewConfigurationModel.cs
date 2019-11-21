namespace Hawk.WebApi.Features.Configuration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Configuration.Configuration;
    using static Hawk.Domain.Currency.Currency;
    using static Hawk.Domain.PaymentMethod.PaymentMethod;
    using static Hawk.Domain.Store.Store;
    using static Hawk.Domain.Tag.Tag;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class NewConfigurationModel
    {
        public NewConfigurationModel(
            string type,
            string paymentMethod,
            string currency,
            string store,
            IEnumerable<string> tags)
        {
            this.Type = type;
            this.PaymentMethod = paymentMethod;
            this.Currency = currency;
            this.Store = store;
            this.Tags = tags;
        }

        [Required]
        public string Type { get; }

        [Required]
        public string PaymentMethod { get; }

        [Required]
        public string Currency { get; }

        [Required]
        public string Store { get; }

        [Required]
        public IEnumerable<string> Tags { get; }

        public static implicit operator NewConfigurationModel(Configuration entity) => new NewConfigurationModel(
            entity.Type,
            entity.PaymentMethod,
            entity.Currency,
            entity.Store,
            entity.Tags.Select(tag => tag.Name));

        public static Option<Configuration> MapNewConfiguration(string description, NewConfigurationModel model)
        {
            var tags = model
                .Tags
                .Select(tag => NewTag(tag))
                .ToList();

            if (tags.Any(tag => tag.IsFailure))
            {
                return None();
            }

            return NewConfiguration(
                model.Type,
                description,
                NewPaymentMethod(model.PaymentMethod),
                NewCurrency(model.Currency),
                NewStore(model.Store),
                new List<Tag>(tags.Select(tag => tag.Get())));
        }
    }
}
