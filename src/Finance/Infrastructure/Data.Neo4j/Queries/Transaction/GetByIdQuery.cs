namespace Finance.Infrastructure.Data.Neo4j.Queries.Transaction
{
    using System.Linq;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure.Data.Neo4j.Mappings;

    public class GetByIdQuery : QueryBase
    {
        private readonly TransactionMapping mapping;

        public GetByIdQuery(Database database, TransactionMapping mapping, GetScript file)
            : base(database, file)
        {
            this.mapping = mapping;
        }

        public virtual Transaction GetResult(int id, string email)
        {
            var query = this.File.ReadAllText(@"Transaction\GetById.cql");
            var parameters = new
            {
                id,
                email
            };

            var entities = this.Database.Execute(this.mapping.MapFrom, query, parameters);

            return entities.FirstOrDefault();
        }
    }
}