namespace Hawk.Infrastructure.Data.Neo4J.Entities.Store
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Store.StoreMapping;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    using static System.String;

    internal sealed class GetStoreByName : IGetStoreByName
    {
        private static readonly Option<string> Statement = ReadAll("Store.GetStoreByName.cql");
        private readonly Database database;

        public GetStoreByName(Database database) => this.database = database;

        public async Task<Try<Option<Store>>> GetResult(string name, string email)
        {
            var parameters = new
            {
                name,
                email,
            };

            var data = await this.database.ExecuteScalar(MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.SelectMany(store => Some(store.Store));
        }
    }
}
