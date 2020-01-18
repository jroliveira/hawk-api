namespace Hawk.WebApi.Features.Configuration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Hawk.Domain.Configuration;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Domain.Configuration.Configuration;
    using static Hawk.Domain.Currency.Currency;
    using static Hawk.Domain.PaymentMethod.PaymentMethod;
    using static Hawk.Domain.Store.Store;
    using static Hawk.Domain.Tag.Tag;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class CreateConfigurationModel
    {
        public CreateConfigurationModel(
            string description,
            string type,
            string paymentMethod,
            string currency,
            string store,
            IEnumerable<string> tags)
        {
            this.Description = description;
            this.Type = type;
            this.PaymentMethod = paymentMethod;
            this.Currency = currency;
            this.Store = store;
            this.Tags = tags;
        }

        [Required]
        public string Description { get; }

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

        public static implicit operator Option<Configuration>(CreateConfigurationModel model) => NewConfiguration(
            model.Type,
            model.Description,
            NewPaymentMethod(model.PaymentMethod),
            NewCurrency(model.Currency),
            NewStore(model.Store),
            Some(model.Tags.Select(tag => NewTag(tag).ToOption())));
    }
}
