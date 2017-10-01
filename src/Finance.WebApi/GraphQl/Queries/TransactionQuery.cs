namespace Finance.WebApi.GraphQl.Queries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using Finance.Infrastructure.Data.Neo4j.GraphQl;
    using Finance.WebApi.GraphQl.Types;

    using GraphQL.Types;

    public class TransactionQuery : ObjectGraphType
    {
        private readonly GetQuery get;
        private readonly GetAllQuery getAll;
        private readonly IMapper mapper;

        public TransactionQuery(
            GetQuery get,
            GetAllQuery getAll,
            IMapper mapper)
        {
            this.get = get;
            this.getAll = getAll;
            this.mapper = mapper;

            this.Field<TransactionType>(
                "transaction",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: this.GetByIdAsync);

            this.Field<ListGraphType<TransactionType>>(
                "transactions",
                resolve: this.GetAllAsync);
        }

        private async Task<Sources.Transaction> GetByIdAsync(ResolveFieldContext<object> context)
        {
            var id = context.GetArgument<string>("id");
            var entity = await this.get.GetResultAsync(id, "junolive@gmail.com").ConfigureAwait(false);

            return this.mapper.Map<Sources.Transaction>(entity);
        }

        private async Task<IEnumerable<Sources.Transaction>> GetAllAsync(ResolveFieldContext<object> context)
        {
            var entity = await this.getAll.GetResultAsync("junolive@gmail.com").ConfigureAwait(false);

            return this.mapper.Map<IEnumerable<Sources.Transaction>>(entity);
        }
    }
}
