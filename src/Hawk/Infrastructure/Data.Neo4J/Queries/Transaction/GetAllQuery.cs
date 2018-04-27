namespace Hawk.Infrastructure.Data.Neo4J.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;
    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Http.Query.Filter;

    internal sealed class GetAllQuery : IGetAllQuery
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("Transaction.GetAll.cql");
        private readonly Database database;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;
        private readonly IWhere<string, Filter> where;

        public GetAllQuery(
            Database database,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
        {
            this.database = database;
            this.limit = limit;
            this.skip = skip;
            this.where = where;
        }

        public async Task<Try<Paged<Transaction>>> GetResult(string email, Filter filter)
        {
            var statement = Statement.GetOrElse(string.Empty).Replace("#where#", this.where.Apply(filter, "transaction"));

            var parameters = new
            {
                email,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.database.Execute(TransactionMapping.MapFrom, statement, parameters).ConfigureAwait(false);

            return data.Match<Try<Paged<Transaction>>>(
                _ => _,
                items => new Paged<Transaction>(items.Select(item => item.GetOrElse(default)).ToList(), parameters.skip, parameters.limit));
        }
    }
}