namespace Hawk.WebApi.Lib.Mappings
{
    using System.Linq;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.WebApi.Models.Store.Get;

    internal static class StoreMapping
    {
        internal static Paged<Store> ToModel(this Paged<Try<(Domain.Entities.Store Store, uint Count)>> @this)
        {
            var model = @this
                .Data
                .Select(item => item.GetOrElse(default))
                .Select(item => new Store(item.Store, item.Count))
                .ToList();

            return new Paged<Store>(model, @this.Skip, @this.Limit);
        }
    }
}
