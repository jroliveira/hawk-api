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

    using Http.Query.Filter;

    using static System.IO.Path;
    using static System.String;

    using static Hawk.Domain.Budget.Data.Neo4J.BudgetMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetBudgetById : Query<GetBudgetByIdParam, Budget>, IGetBudgetById
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Budget", "Data.Neo4J", "Queries", "GetBudgetById.cql"));
        private readonly Neo4JConnection connection;
        private readonly IWhere<string, Filter> where;
        private readonly IWhere<string, Filter> whereTransaction;

        public GetBudgetById(
            Neo4JConnection connection,
            IEnumerable<IWhere<string, Filter>> where)
        {
            where = where.ToList();

            this.connection = connection;
            this.where = where.First(item => item.Name == typeof(Where).FullName);
            this.whereTransaction = where.First(item => item.Name == typeof(WhereTransaction).FullName);
        }

        protected override Task<Try<Budget>> GetResult(GetBudgetByIdParam param) => this.connection.ExecuteCypherScalar(
            MapBudget(param.Filter),
            Statement
                .GetOrElse(Empty)
                .Replace("#where#", this.where.Apply(param.Filter))
                .Replace("#where_transaction#", this.whereTransaction.Apply(param.Filter)),
            new
            {
                email = param.Email.Value,
                id = param.Id.ToString(),
            });
    }
}
