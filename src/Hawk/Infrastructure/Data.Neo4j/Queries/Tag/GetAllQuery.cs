namespace Hawk.Infrastructure.Data.Neo4j.Queries.Tag
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction.Details;
    using Hawk.Infrastructure.Data.Neo4j.Mappings;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllQuery : GetAllQueryBase
    {
        private readonly TagMapping mapping;

        public GetAllQuery(
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

        public virtual async Task<Paged<Tag>> GetResultAsync(string email, Filter filter)
        {
            var query = this.File.ReadAllText(@"Tag.GetAll.cql");
            var parameters = new
            {
                email,
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