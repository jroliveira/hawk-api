namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Transaction.Parcel;

    internal static class ParcelMapping
    {
        private const string Total = "total";
        private const string Number = "number";

        internal static Try<Parcel> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => CreateWith(record.Get<uint>(Total), record.Get<uint>(Number)),
            () => new NullReferenceException("Parcel cannot be null."));
    }
}
