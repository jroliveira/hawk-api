namespace Hawk.WebApi.GraphQl.Queries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using GraphQL.Types;

    using Hawk.Infrastructure.Data.Neo4j.GraphQl.Store;
    using Hawk.WebApi.GraphQl.Sources;
    using Hawk.WebApi.GraphQl.Types;

    public class StoreQuery
    {
        private readonly GetAllQuery getAll;
        private readonly IMapper mapper;

        public StoreQuery(
            GetAllQuery getAll,
            IMapper mapper)
        {
            this.getAll = getAll;
            this.mapper = mapper;
        }

        public void Configure(HawkQuery query)
        {
            query
                .Field<ListGraphType<StoreType>>(
                "stores",
                resolve: this.GetAll);
        }

        private async Task<IEnumerable<Store>> GetAll(ResolveFieldContext<object> context)
        {
            var entities = await this.getAll.GetResult("junolive@gmail.com").ConfigureAwait(false);

            return this.mapper.Map<IEnumerable<Store>>(entities);
        }
    }
}