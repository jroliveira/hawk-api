namespace Hawk.Domain.Shared
{
    using System;

    public abstract class ValueObject<T, TValue> : IEquatable<ValueObject<T, TValue>>
    {
        protected ValueObject(in TValue value) => this.Value = value;

        public TValue Value { get; }

        public static implicit operator TValue(in ValueObject<T, TValue> valueObject) => valueObject.Value;

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

            return obj is T @object && this.Equals(@object);
        }

        public bool Equals(ValueObject<T, TValue> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.Value, other.Value);
        }

        public override int GetHashCode() => this.Value != null ? this.Value.GetHashCode() : 0;
    }
}
