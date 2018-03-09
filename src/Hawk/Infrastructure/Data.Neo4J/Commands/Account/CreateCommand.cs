namespace Hawk.Infrastructure.Data.Neo4J.Commands.Account
{
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Account;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;

    internal sealed class CreateCommand : Connection, ICreateCommand
    {
        private readonly AccountMapping mapping;

        public CreateCommand(Database database, GetScript file, AccountMapping mapping)
            : base(database, file, "Account.Create.cql")
        {
            Guard.NotNull(mapping, nameof(mapping), "Account mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Account> Execute(Account entity)
        {
            var parameters = new
            {
                id = entity.Id.ToString(),
                email = entity.Email,
                creationDate = entity.CreationAt.ToString(CultureInfo.InvariantCulture)
            };

            var inserted = await this.Database.Execute(this.mapping.MapFrom, this.Statement, parameters).ConfigureAwait(false);

            return inserted.FirstOrDefault();
        }
    }
}