namespace Finance.Infrastructure.Data.Neo4j.Commands.Account
{
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities;
    using Finance.Infrastructure.Data.Neo4j.Mappings;

    public class CreateCommand
    {
        private readonly Database database;
        private readonly AccountMapping mapping;
        private readonly File file;

        public CreateCommand(Database database, AccountMapping mapping, File file)
        {
            this.database = database;
            this.mapping = mapping;
            this.file = file;
        }

        public virtual async Task<Account> ExecuteAsync(Account entity)
        {
            entity.HashPassword();

            var query = this.file.ReadAllText(@"Account\create.cql");
            var parameters = new
            {
                email = entity.Email,
                password = entity.Password,
                creationDate = entity.CreationAt.ToString(CultureInfo.InvariantCulture)
            };

            var inserted = this.database.Execute(this.mapping.MapFrom, query, parameters);

            return inserted.FirstOrDefault();
        }
    }
}