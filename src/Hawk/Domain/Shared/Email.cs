namespace Hawk.Domain.Shared
{
    using System;

    public sealed class Email : IEquatable<Email>
    {
        private readonly string value;

        private Email(string value) => this.value = value;

        public static implicit operator string(Email email) => email.value;

        public static implicit operator Email(string value) => new Email(value);

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

            return obj is Email email && this.Equals(email);
        }

        public bool Equals(Email other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.value, other.value);
        }

        public override int GetHashCode() => this.value != null ? this.value.GetHashCode() : 0;

        public override string ToString() => this.value;
    }
}
