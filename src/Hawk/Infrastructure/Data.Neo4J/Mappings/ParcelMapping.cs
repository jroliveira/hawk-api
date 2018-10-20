namespace Hawk.Infrastructure.Data.Neo4J.Mappings
{
    using System;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Entities.Parcel;

    internal static class ParcelMapping
    {
        private const string Total = "total";
        private const string Number = "number";

        internal static Try<Parcel> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => CreateWith(record.Get<uint>(Total), record.Get<uint>(Number)),
            () => new NullReferenceException("Parcel cannot be null."));
    }
}
