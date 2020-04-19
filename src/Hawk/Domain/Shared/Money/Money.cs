namespace Hawk.Domain.Shared.Money
{
    using System;

    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Money : IEquatable<Option<Money>>
    {
        private Money(double value, Currency currency)
        {
            this.Value = value;
            this.Currency = currency;
        }

        public double Value { get; }

        public Currency Currency { get; }

        public static Try<Money> NewMoney(Option<double> value, Option<Currency> currency) =>
            value
            && currency
            ? new Money(value.Get(), currency.Get())
            : Failure<Money>(new InvalidObjectException("Invalid money."));

        public bool Equals(Option<Money> other) => other.Match(
            some => this.Value.Equals(some.Value)
                    && this.Currency.Equals(some.Currency),
            () => false);
    }
}
