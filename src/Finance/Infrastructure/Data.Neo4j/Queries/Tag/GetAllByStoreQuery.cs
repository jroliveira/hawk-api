namespace Finance.Infrastructure.Data.Neo4j.Queries.Tag
{
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities.Transaction.Details;
    using Finance.Infrastructure.Data.Neo4j.Mappings;
    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllByStoreQuery : GetAllQueryBase
    {
        private readonly TagMapping mapping;

        public GetAllByStoreQuery(
            Database database,
            TagMapping mapping,
            GetScript file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file, limit, skip, where)
        {
            this.mapping = mapping;
        }

        public virtual async Task<Paged<Tag>> GetResultAsync(string email, string store, Filter filter)
        {
            var query = this.File.ReadAllText(@"Tag.GetAllByStore.cql");
            var parameters = new
            {
                email,
                store,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = await this.Database.ExecuteAsync(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);
            var entities = data
                .OrderBy(item => item.Name)
                .ToList();

            return new Paged<Tag>(entities, parameters.skip, parameters.limit);
        }
    }
}