namespace Hawk.Domain.Entities
{
    using System;

    using Hawk.Infrastructure;

    public sealed class Tag : IEquatable<Tag>
    {
        public Tag(string name)
        {
            Guard.NotNullNorEmpty(name, nameof(name), "Tag's name cannot be null or empty.");

            this.Name = name;
        }

        public string Name { get; }

        public static implicit operator string(Tag tag)
        {
            return tag.Name;
        }

        public static implicit operator Tag(string name)
        {
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

        public override int GetHashCode()
        {
            return this.Name != null ? this.Name.GetHashCode() : 0;
        }
    }
}