namespace Finance.WebApi.GraphQl.Schemas
{
    using Finance.Infrastructure.Data.Neo4j;
    using Finance.WebApi.GraphQl.Queries;

    using GraphQL.Types;

    public class StarWarsSchema : Schema
    {
        public StarWarsSchema()
        {
            this.Query = new StarWarsQuery(new StarWarsData());
        }
    }
}
