namespace Finance.Infrastructure.Data.Neo4j.Queries.Tag
{
    using System.Linq;

    using Finance.Entities.Transaction.Details;
    using Finance.Infrastructure.Data.Neo4j.Mappings.Transaction;
    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllQuery
    {
        private readonly Database database;
        private readonly TagMapping mapping;
        private readonly File file;

        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;
        private readonly IWhere<string, Filter> where;

        public GetAllQuery(
            Database database,
            TagMapping mapping,
            File file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
        {
            this.database = database;
            this.mapping = mapping;
            this.file = file;
            this.limit = limit;
            this.skip = skip;
            this.where = where;
        }

        public virtual Paged<Tag> GetResult(string email, Filter filter)
        {
            var query = this.file.ReadAllText(@"Tag\GetAll.cql");
            var parameters = new
            {
                email,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter)
            };

            var data = this.database.Execute(this.mapping.MapFrom, query, parameters);
            var entities = data
                .OrderBy(item => item.Name)
                .ToList();

            return new Paged<Tag>(entities, parameters.skip, parameters.limit);
        }
    }
}