namespace Hawk.Infrastructure.Data.Neo4J.Entities.Store
{
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.Store.Store;

    internal static class StoreMapping
    {
        private const string Name = "name";
        private const string Total = "total";
        private const string Data = "data";

        internal static Try<(Store Store, uint Count)> MapFrom(IRecord data) => data.GetRecord(Data).Match(
            record =>
            {
                var total = record.Get<uint>(Total);
                if (!total.IsDefined)
                {
                    return new InvalidObjectException("Invalid store.");
                }

                return MapFrom(record).Match<Try<(Store, uint)>>(
                    _ => _,
                    store => (store, total.Get()));
            },
            () => new NotFoundException("Store not found."));

        internal static Try<Store> MapFrom(Option<Record> record) => record.Match(
            some => CreateWith(some.Get<string>(Name)),
            () => new NotFoundException("Store not found."));
    }
}
