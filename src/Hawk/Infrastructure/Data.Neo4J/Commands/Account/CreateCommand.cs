namespace Hawk.Infrastructure.Data.Neo4J.Commands.Account
{
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Account;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;

    internal sealed class CreateCommand : ICreateCommand
    {
        private readonly Database database;
        private readonly AccountMapping mapping;
        private readonly GetScript file;

        public CreateCommand(Database database, AccountMapping mapping, GetScript file)
        {
            this.database = database;
            this.mapping = mapping;
            this.file = file;
        }

        public async Task<Account> Execute(Account entity)
        {
            var query = this.file.ReadAllText(@"Account.Create.cql");
            var parameters = new
            {
                email = entity.Email,
                creationDate = entity.CreationAt.ToString(CultureInfo.InvariantCulture)
            };

            var inserted = await this.database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);

            return inserted.FirstOrDefault();
        }
    }
}