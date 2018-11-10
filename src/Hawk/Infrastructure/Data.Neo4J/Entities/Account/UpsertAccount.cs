namespace Hawk.Infrastructure.Data.Neo4J.Entities.Account
{
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Account.AccountMapping;

    using static System.Globalization.CultureInfo;
    using static System.String;

    internal sealed class UpsertAccount : IUpsertAccount
    {
        private static readonly Option<string> Statement = ReadAll("Account.UpsertAccount.cql");
        private readonly Database database;

        public UpsertAccount(Database database) => this.database = database;

        public async Task<Try<Account>> Execute(Account entity)
        {
            var parameters = new
            {
                id = entity.Id.ToString(),
                email = entity.Email,
                creationDate = entity.CreationAt.ToString(InvariantCulture),
            };

            var data = await this.database.ExecuteScalar(MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.Lift();
        }
    }
}
