namespace Hawk.Domain.Entities.Payment
{
    using Hawk.Infrastructure;

    public sealed class Method
    {
        public Method(string name, int total = 0)
        {
            Guard.NotNullNorEmpty(name, nameof(name), "Payment method's name cannot be null or empty.");

            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public int Total { get; }

        public static implicit operator string(Method method)
        {
            return method.Name;
        }

        public static implicit operator Method(string name)
        {
            return new Method(name);
        }
    }
}