namespace Hawk.Domain.Payee.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Linq;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;

    using static Hawk.Domain.Payee.Data.Neo4J.PayeeMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetPayeesByCategory : Query<GetPayeesByCategoryParam, Page<Try<Payee>>>, IGetPayeesByCategory
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Payee", "Data.Neo4J", "Queries", "GetPayeesByCategory.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetPayeesByCategory(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        protected override async Task<Try<Page<Try<Payee>>>> GetResult(GetPayeesByCategoryParam param)
        {
            var parameters = new
            {
                email = param.Email.Value,
                category = param.Category.Id,
                skip = this.skip.Apply(param.Filter),
                limit = this.limit.Apply(param.Filter),
            };

            var data = await this.connection.ExecuteCypher(MapPayee, Statement, parameters);

            return data.Select(items => new Page<Try<Payee>>(items, parameters.skip, parameters.limit));
        }
    }
}
