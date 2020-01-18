namespace Hawk.Domain.Payee.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Payee;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Payee.Data.Neo4J.PayeeMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetPayeeByName : IGetPayeeByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Payee", "Data.Neo4J", "GetPayeeByName.cql"));
        private readonly Neo4JConnection connection;

        public GetPayeeByName(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Payee>> GetResult(Email email, string name) => this.connection.ExecuteCypherScalar(
            MapPayee,
            Statement,
            new
            {
                email = email.Value,
                name,
            });
    }
}
