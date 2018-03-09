namespace Hawk.Domain.Entities.Payment
{
    using System;

    using Hawk.Infrastructure;

    public sealed class Method : IEquatable<Method>
    {
        public Method(string name)
        {
            Guard.NotNullNorEmpty(name, nameof(name), "Payment method's name cannot be null or empty.");

            this.Name = name;
        }

        public string Name { get; }

        public static implicit operator string(Method method)
        {
            return method.Name;
        }

        public static implicit operator Method(string name)
        {
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

        public override int GetHashCode()
        {
            return this.Name != null ? this.Name.GetHashCode() : 0;
        }
    }
}