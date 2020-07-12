namespace Hawk.Domain.Account.Data.Neo4J.Commands
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Domain.Account.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.Globalization.CultureInfo;
    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class UpsertAccount : Command<UpsertParam<Guid, Account>>, IUpsertAccount
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Account", "Data.Neo4J", "Commands", "UpsertAccount.cql"));
        private readonly Neo4JConnection connection;

        public UpsertAccount(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(UpsertParam<Guid, Account> param) => this.connection.ExecuteCypher(
            StatementOption,
            new
            {
                id = param.Id.ToString(),
                email = param.Email.Value,
                creationDate = param.Entity.CreationAt.ToString(InvariantCulture),
            });
    }
}
