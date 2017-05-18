namespace Finance.Infrastructure.Data.Neo4j.Queries.Store
{
    using System.Linq;

    using Finance.Entities.Transaction.Details;
    using Finance.Infrastructure.Data.Neo4j.Mappings;
    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllQuery : GetAllQueryBase<Store>
    {
        public GetAllQuery(
            Database database,
            IMapping<Store> mapping,
            File file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, mapping, file, limit, skip, where)
        {
        }

        public virtual Paged<Store> GetResult(string email, Filter filter)
        {
            var query = this.File.ReadAllText(@"Store\GetAll.cql");
            var parameters = new
            {
                email,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = this.Database.Execute(this.Mapping.MapFrom, query, parameters);
            var entities = data
                .OrderBy(item => item.Name)
                .ToList();

            return new Paged<Store>(entities, parameters.skip, parameters.limit);
        }
    }
}