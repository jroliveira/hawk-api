namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteTransaction : IDeleteTransaction
    {
        private static readonly Option<string> Statement = ReadAll("Transaction.DeleteTransaction.cql");
        private readonly Database database;

        public DeleteTransaction(Database database) => this.database = database;

        public Task<Try<Unit>> Execute(Email email, Guid id)
        {
            var parameters = new
            {
                email = email.ToString(),
                id = id.ToString(),
            };

            return this.database.Execute(Statement, parameters);
        }
    }
}
