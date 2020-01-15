namespace Hawk.Domain.Shared
{
    using System;
    using System.Runtime.Serialization;

    using static Hawk.Infrastructure.Security.Md5HashAlgorithm;

    [Serializable]
    public sealed class Email : ValueObject<Email, string>, ISerializable
    {
        private Email(string value)
            : base(value)
        {
        }

        public static implicit operator Email(string value) => new Email(value);

        public void GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue(nameof(Email).ToLower(), ComputeHash(this));
    }
}
