namespace Hawk.Domain.Installment.Data.Neo4J.Queries
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Installment;
    using Hawk.Domain.Installment.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;
    using static System.String;

    using static Hawk.Domain.Installment.Data.Neo4J.InstallmentMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetInstallments : Query<GetAllParam, Page<Try<Installment>>>, IGetInstallments
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Installment", "Data.Neo4J", "Queries", "GetInstallments.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetInstallments(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        protected override async Task<Try<Page<Try<Installment>>>> GetResult(GetAllParam param)
        {
            var parameters = new
            {
                email = param.Email.Value,
                skip = this.skip.Apply(param.Filter),
                limit = this.limit.Apply(param.Filter),
            };

            var data = await this.connection.ExecuteCypher(
                record => MapInstallment(record),
                StatementOption.GetOrElse(Empty),
                parameters);

            return data.Select(items => new Page<Try<Installment>>(items, parameters.skip, parameters.limit));
        }
    }
}
