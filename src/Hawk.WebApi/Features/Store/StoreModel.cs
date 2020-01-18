namespace Hawk.WebApi.Features.Store
{
    using Hawk.Domain.Store;

    public sealed class StoreModel
    {
        private StoreModel(Store entity)
        {
            this.Name = entity.Value;
            this.Transactions = entity.Transactions;
        }

        public string Name { get; }

        public uint Transactions { get; }

        internal static StoreModel NewStoreModel(Store entity) => new StoreModel(entity);
    }
}
