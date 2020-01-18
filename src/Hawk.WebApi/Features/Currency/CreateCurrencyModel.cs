namespace Hawk.WebApi.Features.Currency
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Currency.Currency;

    public class CreateCurrencyModel
    {
        public CreateCurrencyModel(string name) => this.Name = name;

        [Required]
        public string Name { get; }

        public static implicit operator Option<Currency>(CreateCurrencyModel model) => NewCurrency(model.Name);
    }
}
