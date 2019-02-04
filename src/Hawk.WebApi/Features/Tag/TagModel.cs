namespace Hawk.WebApi.Features.Tag
{
    using System.Linq;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using static Hawk.WebApi.Features.Shared.ErrorModels.GenericErrorModel;

    public sealed class TagModel
    {
        public TagModel(string name, uint total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public uint Total { get; }

        internal static Paged<object> MapFrom(Paged<Try<(Tag Tag, uint Count)>> @this)
        {
            var model = @this
                .Data
                .Select(item => item.Match(
                    HandleError,
                    tag => new TagModel(tag.Tag, tag.Count)))
                .ToList();

            return new Paged<object>(model, @this.Skip, @this.Limit);
        }
    }
}
