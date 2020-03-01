namespace Hawk.Domain.Configuration
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Category;
    using Hawk.Domain.Currency;
    using Hawk.Domain.Payee;
    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Configuration : Entity<string>
    {
        private Configuration(
            string type,
            string description,
            PaymentMethod paymentMethod,
            Currency currency,
            Payee payee,
            Category category,
            IEnumerable<Tag> tags)
            : base(description)
        {
            this.Type = type;
            this.PaymentMethod = paymentMethod;
            this.Currency = currency;
            this.Payee = payee;
            this.Category = category;
            this.Tags = tags.ToList();
        }

        public string Type { get; }

        public PaymentMethod PaymentMethod { get; }

        public Currency Currency { get; }

        public Payee Payee { get; }

        public Category Category { get; }

        public IReadOnlyCollection<Tag> Tags { get; }

        public static Try<Configuration> NewConfiguration(
            Option<string> type,
            Option<string> description,
            Option<PaymentMethod> paymentMethod,
            Option<Currency> currency,
            Option<Payee> payee,
            Option<Category> category,
            Option<IEnumerable<Option<Tag>>> tags) =>
                type
                && description
                && paymentMethod
                && currency
                && payee
                && category
                && tags
                && tags.Get().All(_ => _)
                ? new Configuration(
                    type.Get(),
                    description.Get(),
                    paymentMethod.Get(),
                    currency.Get(),
                    payee.Get(),
                    category.Get(),
                    tags.Get().Select(tag => tag.Get()))
                : Failure<Configuration>(new InvalidObjectException("Invalid configuration."));
    }
}
