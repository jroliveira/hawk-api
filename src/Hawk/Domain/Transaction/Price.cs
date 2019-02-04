namespace Hawk.Domain.Transaction
{
    using Hawk.Domain.Currency;
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Price
    {
        private Price(double value, Currency currency)
        {
            this.Value = value;
            this.Currency = currency;
        }

        public double Value { get; }

        public Currency Currency { get; }

        public static Try<Price> CreateWith(Option<double> value, Option<Currency> currency) =>
            value
            && currency
            ? new Price(value.Get(), currency.Get())
            : Failure<Price>(new InvalidObjectException("Invalid price."));
    }
}
