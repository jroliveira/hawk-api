namespace Hawk.WebApi.Features.Tag
{
    using System.Linq;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;

    public sealed class TagModel
    {
        public TagModel(Tag entity)
            : this(entity, default)
        {
        }

        public TagModel(string name, uint total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public uint Total { get; }

        internal static TryModel<Page<TryModel<TagModel>>> MapTag(Page<Try<(Tag Tag, uint Count)>> @this) => new Page<TryModel<TagModel>>(
            @this
                .Data
                .Select(item => item.Match(
                    HandleException<TagModel>,
                    tag => new TagModel(tag.Tag, tag.Count))),
            @this.Skip,
            @this.Limit);
    }
}
