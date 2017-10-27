namespace Hawk.Infrastructure.Data.Neo4j.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction;
    using Hawk.Infrastructure.Data.Neo4j.Mappings;

    public class GetByIdQuery : QueryBase
    {
        private readonly TransactionMapping mapping;

        public GetByIdQuery(Database database, TransactionMapping mapping, GetScript file)
            : base(database, file)
        {
            this.mapping = mapping;
        }

        public virtual async Task<Transaction> GetResultAsync(string id, string email)
        {
            var query = this.File.ReadAllText(@"Transaction.GetById.cql");
            var parameters = new
            {
                id,
                email
            };

            var entities = await this.Database.ExecuteAsync(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);

            return entities.FirstOrDefault();
        }
    }
}