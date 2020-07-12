namespace Hawk.Domain.PaymentMethod.Data.Neo4J.Commands
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.PaymentMethod.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class UpsertPaymentMethod : Command<UpsertParam<string, PaymentMethod>>, IUpsertPaymentMethod
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("PaymentMethod", "Data.Neo4J", "Commands", "UpsertPaymentMethod.cql"));
        private readonly Neo4JConnection connection;

        public UpsertPaymentMethod(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(UpsertParam<string, PaymentMethod> param) => this.connection.ExecuteCypher(
            StatementOption,
            new
            {
                email = param.Email.Value,
                name = param.Id,
                newName = param.Entity.Id,
            });
    }
}
