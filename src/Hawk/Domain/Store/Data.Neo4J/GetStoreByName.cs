namespace Hawk.Domain.Store.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Store.Data.Neo4J.StoreMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetStoreByName : IGetStoreByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Store", "Data.Neo4J", "GetStoreByName.cql"));
        private readonly Neo4JConnection connection;

        public GetStoreByName(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Store>> GetResult(Email email, string name) => this.connection.ExecuteCypherScalar(
            MapStore,
            Statement,
            new
            {
                email = email.Value,
                name,
            });
    }
}
