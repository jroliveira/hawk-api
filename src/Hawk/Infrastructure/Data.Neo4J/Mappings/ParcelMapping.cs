namespace Hawk.Infrastructure.Data.Neo4J.Mappings
{
    using Hawk.Domain.Entities;

    internal sealed class ParcelMapping
    {
        public Parcel MapFrom(Record record)
        {
            if (record == null || !record.Any())
            {
                return null;
            }

            return new Parcel(
                record.Get<int>("total"),
                record.Get<int>("number"));
        }
    }
}