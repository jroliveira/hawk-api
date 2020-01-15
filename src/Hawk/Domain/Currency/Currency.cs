namespace Hawk.Domain.Currency
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Currency : ValueObject<Currency, string>
    {
        private Currency(string name)
            : base(name)
        {
        }

        public static Try<Currency> NewCurrency(Option<string> name) =>
            name
            ? new Currency(name.Get())
            : Failure<Currency>(new InvalidObjectException("Invalid currency."));
    }
}
