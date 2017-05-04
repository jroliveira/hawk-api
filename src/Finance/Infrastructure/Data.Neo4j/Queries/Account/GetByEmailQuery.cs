namespace Finance.Infrastructure.Data.Neo4j.Queries.Account
{
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities;
    using Finance.Infrastructure.Data.Neo4j.Mappings;

    public class GetByEmailQuery
    {
        private readonly Database database;
        private readonly AccountMapping mapping;
        private readonly File file;

        public GetByEmailQuery(Database database, AccountMapping mapping, File file)
        {
            this.database = database;
            this.mapping = mapping;
            this.file = file;
        }

        public virtual async Task<Account> GetResultAsync(string email)
        {
            var query = this.file.ReadAllText(@"Account\get-by-email.cql");
            var parameters = new
            {
                email
            };

            var entities = this.database.Execute(this.mapping.MapFrom, query, parameters);

            return entities.FirstOrDefault();
        }
    }
}