namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteTransaction : IDeleteTransaction
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Transaction", "DeleteTransaction.cql"));
        private readonly Neo4JConnection connection;

        public DeleteTransaction(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Unit>> Execute(Email email, Guid id) => this.connection.ExecuteCypher(
            Statement,
            new
            {
                email = email.ToString(),
                id = id.ToString(),
            });
    }
}
