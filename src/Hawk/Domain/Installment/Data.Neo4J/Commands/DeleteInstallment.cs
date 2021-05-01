namespace Hawk.Domain.Installment.Data.Neo4J.Commands
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Installment.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteInstallment : Command<DeleteParam<Guid>>, IDeleteInstallment
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Installment", "Data.Neo4J", "Commands", "DeleteInstallment.cql"));
        private readonly Neo4JConnection connection;

        public DeleteInstallment(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(DeleteParam<Guid> param) => this.connection.ExecuteCypher(
            StatementOption,
            new
            {
                email = param.Email.Value,
                id = param.Id.ToString(),
            });
    }
}
