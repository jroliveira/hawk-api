namespace Hawk.Domain.Store
{
    using System;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Store : IEquatable<Store>
    {
        private Store(string name) => this.Name = name;

        public string Name { get; }

        public static implicit operator string(Store store) => store.Name;

        public static Try<Store> NewStore(Option<string> name) =>
            name
            ? new Store(name.Get())
            : Failure<Store>(new InvalidObjectException("Invalid store."));

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(default, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is Store store && this.Equals(store);
        }

        public bool Equals(Store other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.Name, other.Name);
        }

        public override int GetHashCode() => this.Name != default ? this.Name.GetHashCode() : 0;
    }
}
