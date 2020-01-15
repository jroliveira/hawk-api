namespace Hawk.Domain.Store
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Store : ValueObject<Store, string>
    {
        private Store(string name)
            : base(name)
        {
        }

        public static Try<Store> NewStore(Option<string> name) =>
            name
            ? new Store(name.Get())
            : Failure<Store>(new InvalidObjectException("Invalid store."));
    }
}
