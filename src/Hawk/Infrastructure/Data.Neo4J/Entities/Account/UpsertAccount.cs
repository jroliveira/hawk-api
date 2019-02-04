namespace Hawk.Infrastructure.Data.Neo4J.Entities.Account
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Monad;

    using static System.Globalization.CultureInfo;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Account.AccountMapping;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertAccount : IUpsertAccount
    {
        private static readonly Option<string> Statement = ReadAll("Account.UpsertAccount.cql");
        private readonly Database database;

        public UpsertAccount(Database database) => this.database = database;

        public Task<Try<Account>> Execute(Option<Account> entity)
        {
            if (!entity.IsDefined)
            {
                return Task(Failure<Account>(new NullReferenceException("Account is required.")));
            }

            var parameters = new
            {
                id = entity.Get().Id.ToString(),
                email = entity.Get().Email.ToString(),
                creationDate = entity.Get().CreationAt.ToString(InvariantCulture),
            };

            return this.database.ExecuteScalar(MapFrom, Statement, parameters);
        }
    }
}
