namespace Hawk.Domain.Transaction.Data.Neo4J.Commands
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared.Commands;
    using Hawk.Domain.Transaction.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteTransaction : Command<DeleteParam<Guid>>, IDeleteTransaction
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Transaction", "Data.Neo4J", "Commands", "DeleteTransaction.cql"));
        private readonly Neo4JConnection connection;

        public DeleteTransaction(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(DeleteParam<Guid> param) => this.connection.ExecuteCypher(
            Statement,
            new
            {
                email = param.Email.Value,
                id = param.Id.ToString(),
            });
    }
}
