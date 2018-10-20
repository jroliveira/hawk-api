namespace Hawk.Domain.Entities
{
    using System;

    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.String;

    public sealed class Tag : IEquatable<Tag>
    {
        private Tag(string name) => this.Name = name;

        public string Name { get; }

        public static implicit operator string(Tag tag) => tag.Name;

        public static Try<Tag> CreateWith(Option<string> nameOption)
        {
            var name = nameOption.GetOrElse(Empty);
            if (IsNullOrEmpty(name))
            {
                return new ArgumentNullException(nameof(name), "Tag's name cannot be null or empty.");
            }

            return new Tag(name);
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

            return string.Equals(this.Name, other.Name);
        }

        public override int GetHashCode() => this.Name != null ? this.Name.GetHashCode() : 0;
    }
}