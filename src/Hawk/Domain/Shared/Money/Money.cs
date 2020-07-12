namespace Hawk.Domain.Shared.Money
{
    using System;
    using System.Linq;

    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Money : IEquatable<Option<Money>>
    {
        private Money(in double value, in Currency currency)
        {
            this.Value = value;
            this.Currency = currency;
        }

        public double Value { get; }

        public Currency Currency { get; }

        public static Try<Money> NewMoney(in Option<double> valueOption, in Option<Currency> currencyOption) =>
            valueOption
            && currencyOption
                ? new Money(
                    valueOption.Get(),
                    currencyOption.Get())
                : Failure<Money>(new InvalidObjectException("Invalid money."));

        public bool Equals(Option<Money> otherOption) => otherOption
            .Fold(false)(other => this.Value.Equals(other.Value)
                                  && this.Currency.Equals(other.Currency));
    }
}
