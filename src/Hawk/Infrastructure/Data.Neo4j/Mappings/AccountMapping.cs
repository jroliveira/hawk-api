namespace Hawk.Infrastructure.Data.Neo4j.Mappings
{
    using Hawk.Entities;
    using Hawk.Infrastructure.Data.Neo4j.Extensions;

    using global::Neo4j.Driver.V1;

    public class AccountMapping
    {
        public Account MapFrom(IRecord data)
        {
            return this.MapFrom(data.GetRecord("data"));
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
