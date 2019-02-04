namespace Hawk.WebApi.Features.Store
{
    using System.Linq;

    using Hawk.Domain.Store;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Store.Store;

    using static Hawk.WebApi.Features.Shared.ErrorModels.GenericErrorModel;

    public sealed class StoreModel
    {
        public StoreModel(Store entity)
            : this(entity.Name, default)
        {
        }

        public StoreModel(string name, uint total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public uint Total { get; }

        public static implicit operator Option<Store>(StoreModel model) => CreateWith(model.Name);

        internal static Paged<object> MapFrom(Paged<Try<(Store Store, uint Count)>> @this)
        {
            var model = @this
                .Data
                .Select(item => item.Match(
                    HandleError,
                    store => new StoreModel(store.Store, store.Count)))
                .ToList();

            return new Paged<object>(model, @this.Skip, @this.Limit);
        }
    }
}
