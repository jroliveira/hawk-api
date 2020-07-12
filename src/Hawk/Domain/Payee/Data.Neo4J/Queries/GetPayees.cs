namespace Hawk.Domain.Payee.Data.Neo4J.Queries
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Payee;
    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;

    using static Hawk.Domain.Payee.Data.Neo4J.PayeeMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetPayees : Query<GetAllParam, Page<Try<Payee>>>, IGetPayees
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Payee", "Data.Neo4J", "Queries", "GetPayees.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetPayees(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        protected override async Task<Try<Page<Try<Payee>>>> GetResult(GetAllParam param)
        {
            var parameters = new
            {
                email = param.Email.Value,
                skip = this.skip.Apply(param.Filter),
                limit = this.limit.Apply(param.Filter),
            };

            var data = await this.connection.ExecuteCypher(
                record => MapPayee(record),
                StatementOption,
                parameters);

            return data.Select(items => new Page<Try<Payee>>(items, parameters.skip, parameters.limit));
        }
    }
}
