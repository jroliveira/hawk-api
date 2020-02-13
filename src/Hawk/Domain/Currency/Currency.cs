namespace Hawk.Domain.Currency
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Currency : ValueObject<Currency, string>
    {
        private Currency(string name, uint transactions)
            : base(name.ToUpperCase()) => this.Transactions = transactions;

        public uint Transactions { get; }

        public static Try<Currency> NewCurrency(
            Option<string> name,
            Option<uint> transactions = default) =>
                name
                ? new Currency(name.Get(), transactions.GetOrElse(default))
                : Failure<Currency>(new InvalidObjectException("Invalid currency."));
    }
}
