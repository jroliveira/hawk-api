namespace Hawk.Domain.Entities.Payment
{
    using System;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using static System.String;

    public sealed class Method : IEquatable<Method>
    {
        private Method(string name) => this.Name = name;

        public string Name { get; }

        public static implicit operator string(Method method) => method.Name;

        public static Try<Method> CreateWith(Option<string> nameOption)
        {
            var name = nameOption.GetOrElse(Empty);

            if (IsNullOrEmpty(name))
            {
                return new ArgumentNullException(nameof(name), "Payment method's name cannot be null or empty.");
            }

            return new Method(name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is Method method && this.Equals(method);
        }

        public bool Equals(Method other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.Name, other.Name);
        }

        public override int GetHashCode() => this.Name != null ? this.Name.GetHashCode() : 0;
    }
}