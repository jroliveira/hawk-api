namespace Hawk.Domain.Transaction.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;
    using static System.String;

    using static Hawk.Domain.Transaction.Data.Neo4J.TransactionMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetTransactions : IGetTransactions
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Transaction", "Data.Neo4J", "GetTransactions.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;
        private readonly IWhere<string, Filter> where;

        public GetTransactions(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
            this.where = where;
        }

        public async Task<Try<Page<Try<Transaction>>>> GetResult(Email email, Filter filter)
        {
            var parameters = new
            {
                email = email.Value,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.connection.ExecuteCypher(
                MapTransaction,
                Statement
                    .GetOrElse(Empty)
                    .Replace("#where#", this.where.Apply(filter, "transaction")),
                parameters);

            return data.Match<Try<Page<Try<Transaction>>>>(
                _ => _,
                items => new Page<Try<Transaction>>(items, parameters.skip, parameters.limit));
        }
    }
}
