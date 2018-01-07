namespace Hawk.WebApi.Models
{
    internal sealed class Tag
    {
        public Tag(string name, int total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public int Total { get; }
    }
}
