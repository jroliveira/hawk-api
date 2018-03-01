namespace Hawk.Domain.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure;

    public sealed class Store
    {
        private readonly ICollection<Tag> tags;

        public Store(string name)
        {
            Guard.NotNullNorEmpty(name, nameof(name), "Store's name cannot be null or empty.");

            this.Name = name;
            this.tags = new List<Tag>();
        }

        public string Name { get; }

        public IReadOnlyCollection<Tag> Tags => this.tags.ToList();

        public static implicit operator string(Store store)
        {
            return store.Name;
        }

        public static implicit operator Store(string name)
        {
            return new Store(name);
        }

        public void AddTag(Tag tag)
        {
            Guard.NotNull(tag, nameof(tag), "Tag cannot be null.");

            this.tags.Add(tag);
        }
    }
}