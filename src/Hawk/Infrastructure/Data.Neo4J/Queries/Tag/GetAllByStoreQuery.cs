namespace Hawk.Infrastructure.Data.Neo4J.Queries.Tag
{
    using System.Linq;
    using System.Threading.Tasks;
    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Tag;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Http.Query.Filter;
    using static System.String;

    internal sealed class GetAllByStoreQuery : IGetAllByStoreQuery
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("Tag.GetAllByStore.cql");
        private readonly Database database;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetAllByStoreQuery(
            Database database,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.database = database;
            this.limit = limit;
            this.skip = skip;
        }

        public async Task<Try<Paged<Try<(Tag Tag, uint Count)>>>> GetResult(string email, string store, Filter filter)
        {
            var parameters = new
            {
                email,
                store,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.database.Execute(TagMapping.MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.Match<Try<Paged<Try<(Tag, uint)>>>>(
                _ => _,
                items => new Paged<Try<(Tag, uint)>>(items.ToList(), parameters.skip, parameters.limit));
        }
    }
}