namespace Hawk.WebApi.Features.Store
{
    using System.Linq;

    using Hawk.Domain.Store;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Infrastructure.Pagination;

    using static Hawk.Domain.Store.Store;

    using static Infrastructure.ErrorHandling.ErrorHandler;

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

        public static implicit operator Option<Store>(StoreModel model) => NewStore(model.Name);

        internal static TryModel<PageModel<TryModel<StoreModel>>> MapStore(Page<Try<(Store Store, uint Count)>> @this) => new PageModel<TryModel<StoreModel>>(
            @this
                .Data
                .Select(item => item.Match(
                    HandleError<StoreModel>,
                    store => new StoreModel(store.Store, store.Count))),
            @this.Skip,
            @this.Limit);
    }
}
