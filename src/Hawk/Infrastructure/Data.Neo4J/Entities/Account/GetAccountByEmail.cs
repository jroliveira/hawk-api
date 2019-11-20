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
        private static readonly Option<string> Statement = ReadAll("Account.GetAccountByEmail.cql");
        private readonly Database database;

        public GetAccountByEmail(Database database) => this.database = database;

        public Task<Try<Account>> GetResult(Email email) => this.database.ExecuteScalar(
            MapFrom,
            Statement,
            new
            {
                email = email.ToString(),
            });
    }
}
