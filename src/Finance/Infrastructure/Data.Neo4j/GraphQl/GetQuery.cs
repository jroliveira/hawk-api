namespace Finance.Infrastructure.Data.Neo4j.GraphQl
{
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure.Data.Neo4j.Mappings;

    public class GetQuery
    {
        private readonly Database database;
        private readonly TransactionMapping mapping;
        private readonly GetScript file;

        public GetQuery(Database database, TransactionMapping mapping, GetScript file)
        {
            this.database = database;
            this.mapping = mapping;
            this.file = file;
        }

        public virtual async Task<Transaction> GetResultAsync(string id, string email)
        {
            var query = this.file.ReadAllText(@"Transaction.GetById.cql");
            var parameters = new
            {
                id,
                email
            };

            var entities = await this.database.ExecuteAsync(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);

            return entities.FirstOrDefault();
        }
    }
}