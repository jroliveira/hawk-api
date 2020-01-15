namespace Hawk.Domain.Account.Data.Neo4J
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.Globalization.CultureInfo;
    using static System.IO.Path;

    using static Hawk.Domain.Account.Data.Neo4J.AccountMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertAccount : IUpsertAccount
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Account", "Data.Neo4J", "UpsertAccount.cql"));
        private readonly Neo4JConnection connection;

        public UpsertAccount(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Account>> Execute(Option<Account> entity) => entity.Match(
            some => this.connection.ExecuteCypherScalar(
                MapAccount,
                Statement,
                new
                {
                    id = some.Id.ToString(),
                    email = some.Email.Value,
                    creationDate = some.CreationAt.ToString(InvariantCulture),
                }),
            () => Task(Failure<Account>(new NullReferenceException("Account is required."))));
    }
}
