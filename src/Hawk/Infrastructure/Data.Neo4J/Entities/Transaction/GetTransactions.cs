﻿namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Transaction.TransactionMapping;

    using static System.String;

    internal sealed class GetTransactions : IGetTransactions
    {
        private static readonly Option<string> Statement = ReadAll("Transaction.GetTransactions.cql");
        private readonly Database database;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;
        private readonly IWhere<string, Filter> where;

        public GetTransactions(
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
            var statement = Statement.GetOrElse(Empty).Replace("#where#", this.where.Apply(filter, "transaction"));

            var parameters = new
            {
                email,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.database.Execute(MapFrom, statement, parameters).ConfigureAwait(false);

            return data.Match<Try<Paged<Transaction>>>(
                _ => _,
                items => new Paged<Transaction>(items.Select(item => item.GetOrElse(default)).ToList(), parameters.skip, parameters.limit));
        }
    }
}
