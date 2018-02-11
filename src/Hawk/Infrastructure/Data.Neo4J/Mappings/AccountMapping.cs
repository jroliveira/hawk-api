namespace Hawk.Infrastructure.Data.Neo4J.Mappings
{
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;

    using Neo4j.Driver.V1;

    internal sealed class AccountMapping
    {
        public Account MapFrom(IRecord data)
        {
            return this.MapFrom(data.GetRecord("data"));
        }

        public Account MapFrom(Record record)
        {
            Guard.NotNull(record, nameof(record), "Account's record cannot be null.");

            return new Account(
                record.GetGuid(),
                record.Get("email"));
        }
    }
}
