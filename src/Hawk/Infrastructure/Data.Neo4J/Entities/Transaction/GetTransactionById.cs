namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Transaction.TransactionMapping;

    internal sealed class GetTransactionById : IGetTransactionById
    {
        private static readonly Option<string> Statement = ReadCypherScript("Transaction\\GetTransactionById.cql");
        private readonly Neo4JConnection connection;

        public GetTransactionById(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Transaction>> GetResult(Email email, Guid id) => this.connection.ExecuteCypherScalar(
            MapTransaction,
            Statement,
            new
            {
                email = email.ToString(),
                id = id.ToString(),
            });
    }
}
