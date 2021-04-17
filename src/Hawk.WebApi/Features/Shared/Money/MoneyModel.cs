namespace Hawk.WebApi.Features.Shared.Money
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Hawk.Domain.Shared.Money;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Shared.Money.Money;

    public sealed class MoneyModel
    {
        public MoneyModel(
            double value,
            CurrencyModel currency)
        {
            this.Value = value;
            this.Currency = currency;
        }

        [Required]
        public double Value { get; }

        [Required]
        public CurrencyModel Currency { get; }

        public static implicit operator MoneyModel(in Money entity) => new MoneyModel(
            entity.Value,
            entity.Currency);

        public static implicit operator MoneyModel?(in Try<Money> entityOption) => entityOption
            .Fold(default(MoneyModel))(entity => new MoneyModel(
                entity.Value,
                entity.Currency));

        public static implicit operator Option<Money>(in MoneyModel model) => NewMoney(
            model.Value,
            model.Currency);
    }
}
