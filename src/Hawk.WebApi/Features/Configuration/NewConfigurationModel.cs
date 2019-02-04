namespace Hawk.WebApi.Features.Configuration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Configuration.Configuration;

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

        public static Option<Configuration> MapFrom(string description, NewConfigurationModel model)
        {
            var tags = model
                .Tags
                .Select(tag => Tag.CreateWith(tag))
                .ToList();

            if (tags.Any(tag => tag.IsFailure))
            {
                return None();
            }

            return CreateWith(
                model.Type,
                description,
                Domain.PaymentMethod.PaymentMethod.CreateWith(model.PaymentMethod),
                Domain.Currency.Currency.CreateWith(model.Currency),
                Domain.Store.Store.CreateWith(model.Store),
                new List<Tag>(tags.Select(tag => tag.Get())));
        }
    }
}
