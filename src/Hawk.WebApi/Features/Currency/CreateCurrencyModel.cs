namespace Hawk.WebApi.Features.Currency
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Currency.Currency;

    public class CreateCurrencyModel
    {
        public CreateCurrencyModel(string code, string symbol)
        {
            this.Code = code;
            this.Symbol = symbol;
        }

        [Required]
        public string Code { get; }

        [Required]
        public string Symbol { get; }

        public static implicit operator Option<Currency>(CreateCurrencyModel model) => NewCurrency(model.Code, model.Symbol);
    }
}
