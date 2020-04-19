namespace Hawk.Domain.Currency
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Currency : Entity<string>
    {
        private Currency(
            string name,
            Option<string> symbol,
            uint transactions)
            : base(name.ToUpperCase())
        {
            this.Symbol = symbol;
            this.Transactions = transactions;
        }

        public Option<string> Symbol { get; }

        public uint Transactions { get; }

        public static Try<Currency> NewCurrency(
            Option<string> name,
            Option<string> symbol = default,
            Option<uint> transactions = default) =>
                name
                ? new Currency(
                    name.Get(),
                    symbol,
                    transactions.GetOrElse(default))
                : Failure<Currency>(new InvalidObjectException("Invalid currency."));
    }
}
