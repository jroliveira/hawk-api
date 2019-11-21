namespace Hawk.Domain.Tag
{
    using System;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Tag : IEquatable<Tag>
    {
        private Tag(string name) => this.Name = name;

        public string Name { get; }

        public static implicit operator string(Tag tag) => tag.Name;

        public static Try<Tag> NewTag(Option<string> name) =>
            name
            ? new Tag(name.Get())
            : Failure<Tag>(new InvalidObjectException("Invalid tag."));

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

            return obj is Tag tag && this.Equals(tag);
        }

        public bool Equals(Tag other)
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

        public override int GetHashCode() => this.Name != null ? this.Name.GetHashCode() : 0;
    }
}
