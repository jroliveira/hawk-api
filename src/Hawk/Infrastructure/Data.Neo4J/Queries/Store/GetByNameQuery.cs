namespace Hawk.Infrastructure.Data.Neo4J.Queries.Store
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Store;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;

    internal sealed class GetByNameQuery : Connection, IGetByNameQuery
    {
        private readonly StoreMapping mapping;

        public GetByNameQuery(Database database, GetScript file, StoreMapping mapping)
            : base(database, file, "Store.GetByName.cql")
        {
            Guard.NotNull(mapping, nameof(mapping), "Transaction mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Store> GetResult(string name, string email)
        {
            var parameters = new
            {
                name,
                email
            };

            var entities = await this.Database.Execute(this.mapping.MapFrom, this.Statement, parameters).ConfigureAwait(false);

            return entities
                .FirstOrDefault()
                .Store;
        }
    }
}