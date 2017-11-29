namespace Hawk.Infrastructure.Data.Neo4j.GraphQl.Store
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction.Details;
    using Hawk.Infrastructure.Data.Neo4j.Mappings;

    public class GetAllQuery
    {
        private readonly Database database;
        private readonly StoreMapping mapping;
        private readonly GetScript file;

        public GetAllQuery(
            Database database,
            StoreMapping mapping,
            GetScript file)
        {
            this.database = database;
            this.mapping = mapping;
            this.file = file;
        }

        public virtual async Task<IEnumerable<Store>> GetResult(string email)
        {
            var query = this.file.ReadAllText(@"Store.GetAll.graphql.cql");
            var parameters = new
            {
                email
            };

            var entities = await this.database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);

            return entities.OrderBy(item => item.Name).ToList();
        }
    }
}