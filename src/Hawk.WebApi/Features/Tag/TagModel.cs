namespace Hawk.WebApi.Features.Tag
{
    using System.Linq;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Infrastructure.Pagination;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;

    public sealed class TagModel
    {
        public TagModel(string name, uint total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public uint Total { get; }

        internal static TryModel<PageModel<TryModel<TagModel>>> MapTag(Page<Try<(Tag Tag, uint Count)>> @this) => new PageModel<TryModel<TagModel>>(
            @this
                .Data
                .Select(item => item.Match(
                    HandleException<TagModel>,
                    tag => new TagModel(tag.Tag, tag.Count))),
            @this.Skip,
            @this.Limit);
    }
}
