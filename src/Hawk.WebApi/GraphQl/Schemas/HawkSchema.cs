namespace Hawk.WebApi.GraphQl.Schemas
{
    using GraphQL.Types;

    using Hawk.WebApi.GraphQl.Queries;

    public class HawkSchema : Schema
    {
        public HawkSchema(HawkQuery query)
        {
            this.Query = query;
        }
    }
}