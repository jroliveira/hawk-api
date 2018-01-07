namespace Hawk.Domain.Entities
{
    public sealed class Store
    {
        public Store(string name, int total = 0)
        {
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