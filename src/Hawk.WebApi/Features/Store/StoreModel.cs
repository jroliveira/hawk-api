namespace Hawk.WebApi.Features.Store
{
    using System.Linq;

    using Hawk.Domain.Store;
    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using static Hawk.Domain.Store.Store;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;

    public sealed class StoreModel
    {
        public StoreModel(Store entity)
            : this(entity, default)
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

        internal static TryModel<Page<TryModel<StoreModel>>> MapStore(Page<Try<(Store Store, uint Count)>> @this) => new Page<TryModel<StoreModel>>(
            @this
                .Data
                .Select(item => item.Match(
                    HandleException<StoreModel>,
                    store => new StoreModel(store.Store, store.Count))),
            @this.Skip,
            @this.Limit);
    }
}
