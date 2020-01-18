namespace Hawk.Domain.Store.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;

    using static Hawk.Domain.Store.Data.Neo4J.StoreMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetStores : IGetStores
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Store", "Data.Neo4J", "GetStores.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetStores(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        public async Task<Try<Page<Try<Store>>>> GetResult(Email email, Filter filter)
        {
            var parameters = new
            {
                email = email.Value,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.connection.ExecuteCypher(MapStore, Statement, parameters);

            return data.Match<Try<Page<Try<Store>>>>(
                _ => _,
                items => new Page<Try<Store>>(items, parameters.skip, parameters.limit));
        }
    }
}
