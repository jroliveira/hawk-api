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
            in string name,
            in Option<string> symbolOption,
            in uint transactions)
            : base(name.ToUpperCase())
        {
            this.SymbolOption = symbolOption;
            this.Transactions = transactions;
        }

        public static Currency DefaultCurrency => new Currency("EUR", default, default);

        public Option<string> SymbolOption { get; }

        public uint Transactions { get; }

        public static Try<Currency> NewCurrency(
            in Option<string> nameOption,
            in Option<string> symbolOption = default,
            in Option<uint> transactionsOption = default) =>
                nameOption
                    ? new Currency(
                        nameOption.Get(),
                        symbolOption,
                        transactionsOption.GetOrElse(default))
                    : Failure<Currency>(new InvalidObjectException("Invalid currency."));
    }
}
