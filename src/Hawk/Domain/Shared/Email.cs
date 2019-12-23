namespace Hawk.Domain.Shared
{
    using System;
    using System.Runtime.Serialization;

    using static Hawk.Infrastructure.Security.Md5HashAlgorithm;

    [Serializable]
    public sealed class Email : IEquatable<Email>, ISerializable
    {
        private readonly string value;

        private Email(string value) => this.value = value;

        public static implicit operator string(Email email) => email.value;

        public static implicit operator Email(string value) => new Email(value);

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

        public override int GetHashCode() => this.value != default ? this.value.GetHashCode() : 0;

        public override string ToString() => this.value;

        public void GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue(nameof(Email).ToLower(), ComputeHash(this.value));
    }
}
