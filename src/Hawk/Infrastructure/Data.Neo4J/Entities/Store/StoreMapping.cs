namespace Hawk.Infrastructure.Data.Neo4J.Entities.Store
{
    using System;
    using System.Linq;

    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.Store.Store;

    internal static class StoreMapping
    {
        private const string Total = "total";
        private const string Name = "name";
        private const string Tags = "tags";

        internal static Try<(Store Store, uint Count)> MapFrom(IRecord data) => MapFrom(data.GetRecord("data"));

        internal static Try<(Store Store, uint Count)> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => CreateWith(record.Get<string>(Name)).Match<Try<(Store, uint)>>(
                _ => _,
                store =>
                {
                    record
                        .GetList(Tags)
                        .Select(tag => Tag.CreateWith(tag).ToOption())
                        .ForEach(tagOption => store.AddTag(tagOption));

                    return (store, record.Get<uint>(Total).GetOrElse(0u));
                }),
            () => new NullReferenceException("Store cannot be null."));
    }
}
