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
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Domain.Payee.Payee;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Configuration : Entity<string>
    {
        private Configuration(
            in string type,
            in string description,
            in PaymentMethod paymentMethod,
            in Currency currency,
            in Payee payee,
            in Category category,
            in IEnumerable<Tag> tags)
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
            in Option<string> typeOption,
            in Option<string> descriptionOption,
            in Option<PaymentMethod> paymentMethodOption,
            in Option<Currency> currencyOption,
            in Option<Payee> payeeOption,
            in Option<Category> categoryOption,
            in Option<IEnumerable<Option<Tag>>> tagsOption) =>
                typeOption
                && descriptionOption
                && paymentMethodOption
                && currencyOption
                && payeeOption
                && categoryOption
                && tagsOption
                && tagsOption.Get().All(_ => _)
                    ? new Configuration(
                        typeOption.Get(),
                        descriptionOption.Get(),
                        paymentMethodOption.Get(),
                        currencyOption.Get(),
                        payeeOption.Get(),
                        categoryOption.Get(),
                        tagsOption.Get().Select(tag => tag.Get()))
                    : Failure<Configuration>(new InvalidObjectException($"Invalid configuration '{descriptionOption.GetOrElse("undefined")}' for payee '{payeeOption.GetOrElse(NewPayee("undefined").Get()).Id}'."));
    }
}
