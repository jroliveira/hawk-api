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
            : base(database, file, "Tag.GetAllByStore.cql", limit, skip, where)
        {
            Guard.NotNull(mapping, nameof(mapping), "Tag mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Paged<(Tag Tag, int Count)>> GetResult(string email, string store, Filter filter)
        {
            var parameters = new
            {
                email,
                store,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = await this.Database.Execute(this.mapping.MapFrom, this.Statement, parameters).ConfigureAwait(false);
            var entities = data
                .OrderBy(item => item.Tag.Name)
                .ToList();

            return new Paged<(Tag, int)>(entities, parameters.skip, parameters.limit);
        }
    }
}