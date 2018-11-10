namespace Hawk.WebApi.Lib.Mappings
{
    using System.Linq;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.WebApi.Models.Tag.Get;

    internal static class TagMapping
    {
        internal static Paged<Tag> ToModel(this Paged<Try<(Domain.Tag.Tag Tag, uint Count)>> @this)
        {
            var model = @this
                .Data
                .Select(item => item.GetOrElse(default))
                .Select(item => new Tag(item.Tag, item.Count))
                .ToList();

            return new Paged<Tag>(model, @this.Skip, @this.Limit);
        }
    }
}
