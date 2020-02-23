namespace Hawk.Domain.Tag.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared.Queries;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Tag.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;

    using static Hawk.Domain.Tag.Data.Neo4J.TagMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetTags : Query<GetAllParam, Page<Try<Tag>>>, IGetTags
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Tag", "Data.Neo4J", "Queries", "GetTags.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetTags(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        protected override async Task<Try<Page<Try<Tag>>>> GetResult(GetAllParam param)
        {
            var parameters = new
            {
                email = param.Email.Value,
                skip = this.skip.Apply(param.Filter),
                limit = this.limit.Apply(param.Filter),
            };

            var data = await this.connection.ExecuteCypher(MapTag, Statement, parameters);

            return data.Match<Try<Page<Try<Tag>>>>(
                _ => _,
                items => new Page<Try<Tag>>(items, parameters.skip, parameters.limit));
        }
    }
}
