namespace Hawk.WebApi.Features.Shared.Money
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.String;

    using static Hawk.Domain.Currency.Currency;

    public sealed class CurrencyModel
    {
        public CurrencyModel(string code, string symbol)
        {
            this.Code = code;
            this.Symbol = symbol;
        }

        [Required]
        public string Code { get; }

        public string Symbol { get; }

        public static implicit operator CurrencyModel(in Currency entity) => new CurrencyModel(
            entity.Id,
            entity.SymbolOption.GetOrElse(Empty));

        public static implicit operator Option<Currency>(in CurrencyModel model) => NewCurrency(
            model.Code,
            model.Symbol);
    }
}
