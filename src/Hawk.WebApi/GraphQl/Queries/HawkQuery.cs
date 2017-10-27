namespace Hawk.WebApi.GraphQl.Queries
{
    using GraphQL.Types;

    public class HawkQuery : ObjectGraphType
    {
        public HawkQuery(
            StoreQuery store,
            TransactionQuery transaction)
        {
            store.Configure(this);
            transaction.Configure(this);
        }
    }
}