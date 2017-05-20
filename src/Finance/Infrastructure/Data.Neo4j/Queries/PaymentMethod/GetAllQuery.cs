namespace Finance.Infrastructure.Data.Neo4j.Queries.PaymentMethod
{
    using System.Linq;

    using Finance.Entities.Transaction.Payment;
    using Finance.Infrastructure.Data.Neo4j.Mappings;
    using Finance.Infrastructure.Data.Neo4j.Mappings.Payment;
    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllQuery : GetAllQueryBase
    {
        private readonly MethodMapping mapping;

        public GetAllQuery(
            Database database,
            MethodMapping mapping,
            File file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file, limit, skip, where)
        {
            this.mapping = mapping;
        }

        public virtual Paged<Method> GetResult(string email, Filter filter)
        {
            var query = this.File.ReadAllText(@"PaymentMethod\GetAll.cql");
            var parameters = new
            {
                email,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = this.Database.Execute(this.mapping.MapFrom, query, parameters);
            var entities = data
                .OrderBy(item => item.Name)
                .ToList();

            return new Paged<Method>(entities, parameters.skip, parameters.limit);
        }
    }
}