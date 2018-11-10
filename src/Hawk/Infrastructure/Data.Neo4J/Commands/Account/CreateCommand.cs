namespace Hawk.Infrastructure.Data.Neo4J.Commands.Account
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Account;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Data.Neo4J.Mappings.AccountMapping;
    using static System.String;

    internal sealed class CreateCommand : ICreateCommand
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("Account.Create.cql");
        private readonly Database database;

        public CreateCommand(Database database) => this.database = database;

        public async Task<Try<Account>> Execute(Account entity)
        {
            var parameters = new
            {
                id = entity.Id.ToString(),
                email = entity.Email,
                creationDate = entity.CreationAt.ToString(CultureInfo.InvariantCulture),
            };

            var data = await this.database.ExecuteScalar(MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.Lift();
        }
    }
}
