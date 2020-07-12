namespace Hawk.Domain.PaymentMethod.Data.Neo4J.Commands
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeletePaymentMethod : Command<DeleteParam<string>>, IDeletePaymentMethod
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("PaymentMethod", "Data.Neo4J", "Commands", "DeletePaymentMethod.cql"));
        private readonly Neo4JConnection connection;

        public DeletePaymentMethod(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(DeleteParam<string> param) => this.connection.ExecuteCypher(
            StatementOption,
            new
            {
                email = param.Email.Value,
                name = param.Id,
            });
    }
}
