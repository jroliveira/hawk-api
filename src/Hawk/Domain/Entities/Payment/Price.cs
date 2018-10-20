namespace Hawk.Domain.Entities.Payment
{
    using System;

    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    public sealed class Price
    {
        private Price(double value, Currency currency)
        {
            this.Value = value;
            this.Currency = currency;
        }

        public double Value { get; }

        public Currency Currency { get; }

        public static Try<Price> CreateWith(Option<double> valueOption, Option<Currency> currencyOption)
        {
            var currency = currencyOption.GetOrElse(default);
            if (currency == null)
            {
                return new ArgumentNullException(nameof(currency), "Price's currency cannot be null.");
            }

            return valueOption.Match<Try<Price>>(
                value => new Price(value, currency),
                () => new ArgumentNullException(nameof(valueOption), "Price's value cannot be null."));
        }
    }
}
