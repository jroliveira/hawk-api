namespace Finance.WebApi.GraphQl.Schemas
{
    using Finance.WebApi.GraphQl.Queries;

    using GraphQL.Types;

    public class TransactionSchema : Schema
    {
        public TransactionSchema(TransactionQuery query)
        {
            this.Query = query;
        }
    }
}
