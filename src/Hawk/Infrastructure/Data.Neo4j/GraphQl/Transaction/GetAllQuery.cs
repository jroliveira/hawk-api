namespace Hawk.Infrastructure.Data.Neo4j.GraphQl.Transaction
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction;
    using Hawk.Infrastructure.Data.Neo4j.Mappings;

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

        public virtual async Task<IEnumerable<Transaction>> GetResult(string email)
        {
            var query = this.file.ReadAllText(@"Transaction.GetAll.cql");
            query = query.Replace("#where#", "1=1");

            var parameters = new
            {
                email,
                skip = 0,
                limit = 10000
            };

            return await this.database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);
        }
    }
}