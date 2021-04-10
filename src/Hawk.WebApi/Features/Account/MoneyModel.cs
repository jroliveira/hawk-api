namespace Hawk.WebApi.Features.Account
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Account;
    using Hawk.WebApi.Features.Shared.Money;

    public sealed class MoneyModel
    {
        private MoneyModel(bool hidden, CurrencyModel currency)
        {
            this.Hidden = hidden;
            this.Currency = currency;
        }

        [Required]
        public bool Hidden { get; }

        [Required]
        public CurrencyModel Currency { get; }

        public static implicit operator MoneyModel(in Money entity) => new MoneyModel(
            entity.Hidden,
            entity.Currency);
    }
}
