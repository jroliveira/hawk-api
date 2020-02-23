namespace Hawk.Domain.Transaction.Data.Neo4J.Queries
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared.Queries;
    using Hawk.Domain.Transaction;
    using Hawk.Domain.Transaction.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Transaction.Data.Neo4J.TransactionMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetTransactionById : Query<GetByIdParam<Guid>, Transaction>, IGetTransactionById
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Transaction", "Data.Neo4J", "Queries", "GetTransactionById.cql"));
        private readonly Neo4JConnection connection;

        public GetTransactionById(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Transaction>> GetResult(GetByIdParam<Guid> param) => this.connection.ExecuteCypherScalar(
            MapTransaction,
            Statement,
            new
            {
                email = param.Email.Value,
                id = param.Id.ToString(),
            });
    }
}
