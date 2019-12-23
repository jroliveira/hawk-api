namespace Hawk.Domain.Currency
{
    using System;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Currency : IEquatable<Currency>
    {
        private Currency(string name) => this.Name = name;

        public string Name { get; }

        public static implicit operator string(Currency currency) => currency.Name;

        public static Try<Currency> NewCurrency(Option<string> name) =>
            name
            ? new Currency(name.Get())
            : Failure<Currency>(new InvalidObjectException("Invalid currency."));

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

            return obj is Currency currency && this.Equals(currency);
        }

        public bool Equals(Currency other)
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
