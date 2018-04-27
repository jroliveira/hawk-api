namespace Hawk.Infrastructure.Data.Neo4J.Queries.Store
{
    using System.Linq;
    using System.Threading.Tasks;
    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Store;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static System.String;

    internal sealed class GetByNameQuery : IGetByNameQuery
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("Store.GetByName.cql");
        private readonly Database database;

        public GetByNameQuery(Database database) => this.database = database;

        public async Task<Try<Option<Store>>> GetResult(string name, string email)
        {
            var parameters = new
            {
                name,
                email,
            };

            var data = await this.database.ExecuteScalar(StoreMapping.MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.SelectMany(store => Some(store.Store));
        }
    }
}