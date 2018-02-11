namespace Hawk.Infrastructure.Data.Neo4J.Queries.Tag
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Tag;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    internal sealed class GetAllByStoreQuery : GetAllQueryBase, IGetAllByStoreQuery
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
            Guard.NotNull(mapping, nameof(mapping), "Tag mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Paged<Tag>> GetResult(string email, string store, Filter filter)
        {
            var query = this.File.ReadAllText(@"Tag.GetAllByStore.cql");
            var parameters = new
            {
                email,
                store,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = await this.Database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);
            var entities = data
                .OrderBy(item => item.Name)
                .ToList();

            return new Paged<Tag>(entities, parameters.skip, parameters.limit);
        }
    }
}