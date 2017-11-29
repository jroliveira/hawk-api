namespace Hawk.Infrastructure.Data.Neo4j.GraphQl.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction;
    using Hawk.Infrastructure.Data.Neo4j.Mappings;

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

        public virtual async Task<Transaction> GetResult(string id, string email)
        {
            var query = this.file.ReadAllText(@"Transaction.GetById.cql");
            var parameters = new
            {
                id,
                email
            };

            var entities = await this.database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);

            return entities.FirstOrDefault();
        }
    }
}