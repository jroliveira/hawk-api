namespace Hawk.Domain.Entities.Payment
{
    public sealed class Method
    {
        public Method(string name, int total = 0)
        {
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