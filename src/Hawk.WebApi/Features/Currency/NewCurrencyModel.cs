namespace Hawk.WebApi.Features.Currency
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Currency.Currency;

    public class NewCurrencyModel
    {
        public NewCurrencyModel(string name) => this.Name = name;

        [Required]
        public string Name { get; }

        public static implicit operator Option<Currency>(NewCurrencyModel model) => MapNewCurrency(model);

        public static implicit operator NewCurrencyModel(Currency entity) => new NewCurrencyModel(entity);

        public static Option<Currency> MapNewCurrency(NewCurrencyModel model) => NewCurrency(model.Name);
    }
}
