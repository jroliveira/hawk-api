namespace Hawk.Infrastructure.Data.Neo4J.Entities.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Store.StoreMapping;

    internal sealed class GetStoreByName : IGetStoreByName
    {
        private static readonly Option<string> Statement = ReadCypherScript("Store\\GetStoreByName.cql");
        private readonly Neo4JConnection connection;

        public GetStoreByName(Neo4JConnection connection) => this.connection = connection;

        public async Task<Try<Store>> GetResult(Email email, string name)
        {
            var data = await this.connection.ExecuteCypherScalar(
                MapStore,
                Statement,
                new
                {
                    email = email.ToString(),
                    name,
                });

            return data.Match<Try<Store>>(
                _ => _,
                store => store.Store);
        }
    }
}
