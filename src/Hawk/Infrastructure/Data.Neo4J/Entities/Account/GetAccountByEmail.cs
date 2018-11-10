namespace Hawk.Infrastructure.Data.Neo4J.Entities.Account
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Account.AccountMapping;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    using static System.String;

    internal sealed class GetAccountByEmail : IGetAccountByEmail
    {
        private static readonly Option<string> Statement = ReadAll("Account.GetAccountByEmail.cql");
        private readonly Database database;

        public GetAccountByEmail(Database database) => this.database = database;

        public async Task<Try<Option<Account>>> GetResult(string email)
        {
            var parameters = new
            {
                email,
            };

            var data = await this.database.ExecuteScalar(MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.SelectMany(Some);
        }
    }
}
