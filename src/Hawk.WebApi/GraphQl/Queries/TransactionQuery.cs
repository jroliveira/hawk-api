namespace Hawk.WebApi.GraphQl.Queries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using GraphQL.Types;

    using Hawk.Infrastructure.Data.Neo4j.GraphQl.Transaction;
    using Hawk.WebApi.GraphQl.Sources;
    using Hawk.WebApi.GraphQl.Types;

    public class TransactionQuery
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
        }

        public void Configure(HawkQuery query)
        {
            query
                .Field<TransactionType>(
                "transaction",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: this.GetById);

            query
                .Field<ListGraphType<TransactionType>>(
                "transactions",
                resolve: this.GetAll);
        }

        private async Task<Transaction> GetById(ResolveFieldContext<object> context)
        {
            var id = context.GetArgument<string>("id");
            var entity = await this.get.GetResult(id, "junolive@gmail.com").ConfigureAwait(false);

            return this.mapper.Map<Transaction>(entity);
        }

        private async Task<IEnumerable<Transaction>> GetAll(ResolveFieldContext<object> context)
        {
            var entities = await this.getAll.GetResult("junolive@gmail.com").ConfigureAwait(false);

            return this.mapper.Map<IEnumerable<Transaction>>(entities);
        }
    }
}
