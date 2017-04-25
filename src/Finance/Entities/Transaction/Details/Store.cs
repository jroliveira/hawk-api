namespace Finance.Entities.Transaction.Details
{
    public class Store
    {
        public Store(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

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