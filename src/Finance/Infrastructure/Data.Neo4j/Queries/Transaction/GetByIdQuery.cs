namespace Finance.Infrastructure.Data.Neo4j.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure.Data.Neo4j.Mappings.Transaction;

    public class GetByIdQuery
    {
        private readonly Database database;
        private readonly TransactionMapping mapping;
        private readonly File file;

        public GetByIdQuery(Database database, TransactionMapping mapping, File file)
        {
            this.database = database;
            this.mapping = mapping;
            this.file = file;
        }

        public virtual async Task<Transaction> GetResultAsync(int id, string email)
        {
            var query = this.file.ReadAllText(@"Transaction\get-by-id.cql");
            var parameters = new
            {
                id,
                email
            };

            var entities = await this.database.ExecuteAsync(this.mapping.MapFrom, query, parameters);

            return entities.FirstOrDefault();
        }
    }
}