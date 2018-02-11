namespace Hawk.Domain.Entities.Payment
{
    using Hawk.Infrastructure;

    public sealed class Currency
    {
        public Currency(string name)
        {
            Guard.NotNullNorEmpty(name, nameof(name), "Currency's name cannot be null or empty.");

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