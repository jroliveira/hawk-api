namespace Hawk.Domain.Account
{
    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Utils;

    using static Hawk.Domain.Currency.Currency;

    public sealed class Money
    {
        private Money(
            in bool hidden,
            in Currency currency)
        {
            this.Hidden = hidden;
            this.Currency = currency;
        }

        public static Money DefaultMoney => new Money(
            hidden: default,
            DefaultCurrency);

        public bool Hidden { get; }

        public Currency Currency { get; }

        public static Try<Money> NewMoney(
            in Option<bool> hiddenOption,
            in Option<Currency> currencyOption) =>
                hiddenOption
                && currencyOption
                    ? new Money(
                        hiddenOption.Get(),
                        currencyOption.Get())
                    : Util.Failure<Money>(new InvalidObjectException("Invalid money."));
    }
}
