namespace Hawk.Domain.Payee.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.Payee;
    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Payee.Data.Neo4J.PayeeMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetPayeeByName : Query<GetByIdParam<string>, Payee>, IGetPayeeByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Payee", "Data.Neo4J", "Queries", "GetPayeeByName.cql"));
        private readonly Neo4JConnection connection;

        public GetPayeeByName(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Payee>> GetResult(GetByIdParam<string> param) => this.connection.ExecuteCypherScalar(
            MapPayee,
            Statement,
            new
            {
                email = param.Email.Value,
                name = param.Id,
            });
    }
}
