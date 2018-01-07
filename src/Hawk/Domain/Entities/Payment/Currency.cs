namespace Hawk.Domain.Entities.Payment
{
    public sealed class Currency
    {
        public Currency(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public static implicit operator string(Currency method)
        {
            return method.Name;
        }

        public static implicit operator Currency(string name)
        {
            return new Currency(name);
        }
    }
}