namespace Hawk.Domain.Transaction.Data.Neo4J
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class DeleteTransaction : IDeleteTransaction
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Transaction", "Data.Neo4J", "DeleteTransaction.cql"));
        private readonly Neo4JConnection connection;

        public DeleteTransaction(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Unit>> Execute(Option<Email> email, Option<Guid> id) =>
            email
            && id
                ? this.connection.ExecuteCypher(
                    Statement,
                    new
                    {
                        email = email.Get().Value,
                        id = id.Get().ToString(),
                    })
                : Task(Failure<Unit>(new NullObjectException("Parameters are required.")));
    }
}
