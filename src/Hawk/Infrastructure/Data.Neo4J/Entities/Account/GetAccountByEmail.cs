namespace Hawk.Infrastructure.Data.Neo4J.Entities.Account
{
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Account.AccountMapping;

    internal sealed class GetAccountByEmail : IGetAccountByEmail
    {
        private static readonly Option<string> Statement = ReadCypherScript("Account.GetAccountByEmail.cql");
        private readonly Neo4JConnection connection;

        public GetAccountByEmail(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Account>> GetResult(Email email) => this.connection.ExecuteCypherScalar(
            MapAccount,
            Statement,
            new
            {
                email = email.ToString(),
            });
    }
}
