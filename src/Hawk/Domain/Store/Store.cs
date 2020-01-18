namespace Hawk.Domain.Store
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Store : ValueObject<Store, string>
    {
        private Store(string name, uint transactions)
            : base(name) => this.Transactions = transactions;

        public uint Transactions { get; }

        public static Try<Store> NewStore(
            Option<string> name,
            Option<uint> transactions = default) =>
                name
                ? new Store(name.Get(), transactions.GetOrElse(default))
                : Failure<Store>(new InvalidObjectException("Invalid store."));
    }
}
