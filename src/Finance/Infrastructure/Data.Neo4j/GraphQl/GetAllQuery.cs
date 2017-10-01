namespace Finance.Infrastructure.Data.Neo4j.GraphQl
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure.Data.Neo4j.Mappings;

    public class GetAllQuery
    {
        private readonly Database database;
        private readonly TransactionMapping mapping;
        private readonly GetScript file;

        public GetAllQuery(
            Database database,
            TransactionMapping mapping,
            GetScript file)
        {
            this.database = database;
            this.mapping = mapping;
            this.file = file;
        }

        public virtual async Task<IEnumerable<Transaction>> GetResultAsync(string email)
        {
            var query = this.file.ReadAllText(@"Transaction.GetAll.cql");
            query = query.Replace("#where#", "1=1");

            var parameters = new
            {
                email,
                skip = 0,
                limit = 10000
            };

            return await this.database.ExecuteAsync(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);
        }
    }
}