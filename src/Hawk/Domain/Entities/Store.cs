namespace Hawk.Domain.Entities
{
    using Hawk.Infrastructure;

    public sealed class Store
    {
        public Store(string name, int total = 0)
        {
            Guard.NotNullNorEmpty(name, nameof(name), "Store's name cannot be null or empty.");

            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public int Total { get; }

        public static implicit operator string(Store store)
        {
            return store.Name;
        }

        public static implicit operator Store(string name)
        {
            return new Store(name);
        }
    }
}