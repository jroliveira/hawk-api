namespace Hawk.Domain.Account.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Domain.Account.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Account.Data.Neo4J.AccountMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetAccountByEmail : Query<GetAccountByEmailParam, Account>, IGetAccountByEmail
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Account", "Data.Neo4J", "Queries", "GetAccountByEmail.cql"));
        private readonly Neo4JConnection connection;

        public GetAccountByEmail(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Account>> GetResult(GetAccountByEmailParam param) => this.connection.ExecuteCypherScalar(
            MapAccount,
            Statement,
            new
            {
                email = param.Email.Value,
            });
    }
}
