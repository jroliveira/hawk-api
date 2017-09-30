namespace Finance.WebApi.GraphQl.Queries
{
    using Finance.Infrastructure.Data.Neo4j;
    using Finance.WebApi.GraphQl.Types;

    using GraphQL.Types;

    public class StarWarsQuery : ObjectGraphType
    {
        public StarWarsQuery(StarWarsData data)
        {
            this.Field<DroidType>(
                "droid",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<string>("id");
                    return data.GetByIdAsync(id);
                });
        }
    }
}
