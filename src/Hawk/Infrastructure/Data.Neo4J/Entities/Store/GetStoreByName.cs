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
        private static readonly Option<string> Statement = ReadAll("Store.GetStoreByName.cql");
        private readonly Database database;

        public GetStoreByName(Database database) => this.database = database;

        public async Task<Try<Store>> GetResult(Email email, string name)
        {
            var parameters = new
            {
                email = email.ToString(),
                name,
            };

            var data = await this.database.ExecuteScalar(MapFrom, Statement, parameters).ConfigureAwait(false);

            return data.Match<Try<Store>>(
                _ => _,
                store => store.Store);
        }
    }
}
