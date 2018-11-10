namespace Hawk.Infrastructure.Data.Neo4J.Entities.Store
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Store.StoreMapping;

    using static System.String;

    internal sealed class GetStores : IGetStores
    {
        private static readonly Option<string> Statement = ReadAll("Store.GetStores.cql");
        private readonly Database database;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetStores(
            Database database,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.database = database;
            this.limit = limit;
            this.skip = skip;
        }

        public async Task<Try<Paged<Try<(Store Store, uint Count)>>>> GetResult(string email, Filter filter)
        {
            var parameters = new
            {
                email,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.database.Execute(MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.Match<Try<Paged<Try<(Store, uint)>>>>(
                _ => _,
                items => new Paged<Try<(Store, uint)>>(items.ToList(), parameters.skip, parameters.limit));
        }
    }
}
