namespace Hawk.WebApi.Features.Tag
{
    using Hawk.Domain.Tag;

    public sealed class TagModel
    {
        private TagModel(Tag entity)
        {
            this.Name = entity.Id;
            this.Transactions = entity.Transactions;
        }

        public string Name { get; }

        public uint Transactions { get; }

        internal static TagModel NewTagModel(in Tag entity) => new TagModel(entity);
    }
}
