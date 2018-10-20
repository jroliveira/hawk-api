namespace Hawk.Infrastructure.Data.Neo4J.Queries.Account
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Account;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.String;
    using static Hawk.Infrastructure.Data.Neo4J.Mappings.AccountMapping;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class GetByEmailQuery : IGetByEmailQuery
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("Account.GetByEmail.cql");
        private readonly Database database;

        public GetByEmailQuery(Database database) => this.database = database;

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
