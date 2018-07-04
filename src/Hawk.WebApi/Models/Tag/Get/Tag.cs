namespace Hawk.WebApi.Models.Tag.Get
{
    public sealed class Tag
    {
        public Tag(string name, uint total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public uint Total { get; }
    }
}
