namespace Hawk.Domain.Transaction.Data.Neo4J
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Transaction.Data.Neo4J.TransactionMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetTransactionById : IGetTransactionById
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Transaction", "Data.Neo4J", "GetTransactionById.cql"));
        private readonly Neo4JConnection connection;

        public GetTransactionById(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Transaction>> GetResult(Email email, Guid id) => this.connection.ExecuteCypherScalar(
            MapTransaction,
            Statement,
            new
            {
                email = email.Value,
                id = id.ToString(),
            });
    }
}
