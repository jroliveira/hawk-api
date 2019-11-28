namespace Hawk.Infrastructure.Data.Neo4J.Entities.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Store.StoreMapping;

    internal sealed class GetStores : IGetStores
    {
        private static readonly Option<string> Statement = ReadCypherScript("Store\\GetStores.cql");
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

        public async Task<Try<Page<Try<(Store Store, uint Count)>>>> GetResult(Email email, Filter filter)
        {
            var parameters = new
            {
                email = email.ToString(),
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.connection.ExecuteCypher(MapStore, Statement, parameters);

            return data.Match<Try<Page<Try<(Store, uint)>>>>(
                _ => _,
                items => new Page<Try<(Store, uint)>>(items, parameters.skip, parameters.limit));
        }
    }
}
