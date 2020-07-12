namespace Hawk.Domain.Category.Data.Neo4J.Queries
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;

    using static Hawk.Domain.Category.Data.Neo4J.CategoryMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetCategoriesByPayee : Query<GetCategoriesByPayeeParam, Page<Try<Category>>>, IGetCategoriesByPayee
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Category", "Data.Neo4J", "Queries", "GetCategoriesByPayee.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetCategoriesByPayee(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        protected override async Task<Try<Page<Try<Category>>>> GetResult(GetCategoriesByPayeeParam param)
        {
            var parameters = new
            {
                email = param.Email.Value,
                payee = param.Payee.Id,
                skip = this.skip.Apply(param.Filter),
                limit = this.limit.Apply(param.Filter),
            };

            var data = await this.connection.ExecuteCypher(
                record => MapCategory(record),
                StatementOption,
                parameters);

            return data.Select(items => new Page<Try<Category>>(items, parameters.skip, parameters.limit));
        }
    }
}
