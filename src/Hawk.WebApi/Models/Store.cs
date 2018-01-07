namespace Hawk.WebApi.Models
{
    internal sealed class Store
    {
        public Store(string name, int total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public int Total { get; }
    }
}
