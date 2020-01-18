namespace Hawk.Domain.Store.Data.Neo4J
{
    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Store.Store;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;

    internal static class StoreMapping
    {
        internal static Try<Store> MapStore(IRecord data) => MapStore(MapRecord(data, "data"));

        internal static Try<Store> MapStore(Option<Neo4JRecord> record) => record.Match(
            some => NewStore(some.Get<string>("name"), some.Get<uint>("transactions")),
            () => new NotFoundException("Store not found."));
    }
}
