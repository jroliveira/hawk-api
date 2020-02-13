namespace Hawk.Domain.Payee
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Payee : ValueObject<Payee, string>
    {
        private Payee(string name, uint transactions)
            : base(name.ToPascalCase()) => this.Transactions = transactions;

        public uint Transactions { get; }

        public static Try<Payee> NewPayee(
            Option<string> name,
            Option<uint> transactions = default) =>
                name
                ? new Payee(name.Get(), transactions.GetOrElse(default))
                : Failure<Payee>(new InvalidObjectException("Invalid payee."));
    }
}
