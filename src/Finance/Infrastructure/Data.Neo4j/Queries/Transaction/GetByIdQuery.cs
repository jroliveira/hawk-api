namespace Finance.Infrastructure.Data.Neo4j.Queries.Transaction
{
    using System.Linq;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure.Data.Neo4j.Mappings;
    using Finance.Infrastructure.Data.Neo4j.Mappings.Transaction;

    public class GetByIdQuery : QueryBase<Transaction>
    {
        public GetByIdQuery(Database database, IMapping<Transaction> mapping, File file)
            : base(database, mapping, file)
        {
        }

        public virtual Transaction GetResult(int id, string email)
        {
            var query = this.File.ReadAllText(@"Transaction\GetById.cql");
            var parameters = new
            {
                id,
                email
            };

            var entities = this.Database.Execute(this.Mapping.MapFrom, query, parameters);

            return entities.FirstOrDefault();
        }
    }
}