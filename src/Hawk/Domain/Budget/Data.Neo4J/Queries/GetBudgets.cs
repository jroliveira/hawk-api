namespace Hawk.Domain.Budget.Data.Neo4J.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Budget;
    using Hawk.Domain.Budget.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;
    using static System.String;

    using static Hawk.Domain.Budget.Data.Neo4J.BudgetMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetBudgets : Query<GetAllParam, Page<Try<Budget>>>, IGetBudgets
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Budget", "Data.Neo4J", "Queries", "GetBudgets.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;
        private readonly IWhere<string, Filter> where;
        private readonly IWhere<string, Filter> whereTransaction;

        public GetBudgets(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IEnumerable<IWhere<string, Filter>> where)
        {
            where = where.ToList();

            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
            this.where = where.First(item => item.Name == typeof(Where).FullName);
            this.whereTransaction = where.First(item => item.Name == typeof(WhereTransaction).FullName);
        }

        protected override async Task<Try<Page<Try<Budget>>>> GetResult(GetAllParam param)
        {
            var parameters = new
            {
                email = param.Email.Value,
                skip = this.skip.Apply(param.Filter),
                limit = this.limit.Apply(param.Filter),
            };

            var data = await this.connection.ExecuteCypher(
                MapBudget(param.Filter),
                Statement
                    .GetOrElse(Empty)
                    .Replace("#where#", this.where.Apply(param.Filter))
                    .Replace("#where_transaction#", this.whereTransaction.Apply(param.Filter)),
                parameters);

            return data.Select(items => new Page<Try<Budget>>(items, parameters.skip, parameters.limit));
        }
    }
}
