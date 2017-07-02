namespace Finance.Infrastructure.Data.Neo4j.Mappings
{
    using Finance.Entities;
    using Finance.Infrastructure.Data.Neo4j.Extensions;

    using global::Neo4j.Driver.V1;

    public class AccountMapping
    {
        public Account MapFrom(IRecord record)
        {
            return this.MapFrom(record.GetRecord("data"));
        }

        public Account MapFrom(Record record)
        {
            return new Account(
                record.GetGuid(),
                record.Get("email"),
                record.Get("password"));
        }
    }
}
