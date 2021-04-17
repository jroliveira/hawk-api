namespace Hawk.Domain.Budget.Data.Neo4J.Commands
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Budget.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteBudget : Command<DeleteParam<Guid>>, IDeleteBudget
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Budget", "Data.Neo4J", "Commands", "DeleteBudget.cql"));
        private readonly Neo4JConnection connection;

        public DeleteBudget(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(DeleteParam<Guid> param) => this.connection.ExecuteCypher(
            Statement,
            new
            {
                email = param.Email.Value,
                id = param.Id.ToString(),
            });
    }
}
