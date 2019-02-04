namespace Hawk.Infrastructure.Data.Neo4J.Entities.Tag
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Tag.TagMapping;

    internal sealed class GetTags : IGetTags
    {
        private static readonly Option<string> Statement = ReadAll("Tag.GetTags.cql");
        private readonly Database database;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetTags(
            Database database,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.database = database;
            this.limit = limit;
            this.skip = skip;
        }

        public async Task<Try<Paged<Try<(Tag Tag, uint Count)>>>> GetResult(Email email, Filter filter)
        {
            var parameters = new
            {
                email = email.ToString(),
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.database.Execute(MapFrom, Statement, parameters).ConfigureAwait(false);

            return data.Match<Try<Paged<Try<(Tag, uint)>>>>(
                _ => _,
                items => new Paged<Try<(Tag, uint)>>(items.ToList(), parameters.skip, parameters.limit));
        }
    }
}
