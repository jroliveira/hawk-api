namespace Hawk.Domain.Account.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Account.Data.Neo4J.AccountMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetAccountByEmail : IGetAccountByEmail
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Account", "Data.Neo4J", "GetAccountByEmail.cql"));
        private readonly Neo4JConnection connection;

        public GetAccountByEmail(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Account>> GetResult(Email email) => this.connection.ExecuteCypherScalar(
            MapAccount,
            Statement,
            new
            {
                email = email.Value,
            });
    }
}
