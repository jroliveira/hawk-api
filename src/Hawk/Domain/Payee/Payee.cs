namespace Hawk.Domain.Payee
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Payee : Entity<string>
    {
        private Payee(in string name, in uint transactions)
            : base(name.ToPascalCase()) => this.Transactions = transactions;

        public uint Transactions { get; }

        public static Try<Payee> NewPayee(in Option<string> nameOption, in Option<uint> transactionsOption = default) =>
            nameOption
                ? new Payee(
                    nameOption.Get(),
                    transactionsOption.GetOrElse(default))
                : Failure<Payee>(new InvalidObjectException("Invalid payee."));
    }
}
