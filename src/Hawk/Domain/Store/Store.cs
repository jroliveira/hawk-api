namespace Hawk.Domain.Store
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    using static System.String;

    public sealed class Store
    {
        private readonly ICollection<Tag> tags = new List<Tag>();

        private Store(string name) => this.Name = name;

        public string Name { get; }

        public IReadOnlyCollection<Tag> Tags => this.tags.ToList();

        public static implicit operator string(Store store) => store.Name;

        public static Try<Store> CreateWith(Option<string> nameOption)
        {
            var name = nameOption.GetOrElse(Empty);
            if (IsNullOrEmpty(name))
            {
                return new ArgumentNullException(nameof(name), "Store's name cannot be null or empty.");
            }

            return new Store(name);
        }

        public Try<Unit> AddTag(Option<Tag> tagOption) => this.AddTag(tagOption.GetOrElse(default));

        public Try<Unit> AddTag(Tag tag)
        {
            if (tag == null)
            {
                return new ArgumentNullException(nameof(tag), "Store's tag cannot be null.");
            }

            this.tags.Add(tag);

            return Unit();
        }
    }
}
