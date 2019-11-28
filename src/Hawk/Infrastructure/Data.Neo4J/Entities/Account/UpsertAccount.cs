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
        private static readonly Option<string> Statement = ReadCypherScript("Account\\UpsertAccount.cql");
        private readonly Neo4JConnection connection;

        public UpsertAccount(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Account>> Execute(Option<Account> entity)
        {
            if (!entity.IsDefined)
            {
                return Task(Failure<Account>(new NullReferenceException("Account is required.")));
            }

            return this.connection.ExecuteCypherScalar(
                MapAccount,
                Statement,
                new
                {
                    id = entity.Get().Id.ToString(),
                    email = entity.Get().Email.ToString(),
                    creationDate = entity.Get().CreationAt.ToString(InvariantCulture),
                });
        }
    }
}
