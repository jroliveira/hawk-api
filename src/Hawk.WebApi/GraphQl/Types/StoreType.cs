namespace Hawk.WebApi.GraphQl.Types
{
    using GraphQL.Types;

    using Hawk.WebApi.GraphQl.Sources;

    public class StoreType : ObjectGraphType<Store>
    {
        public StoreType()
        {
            this.Field(transaction => transaction.Name);
            this.Field<TransactionsType>("transactions");
        }
    }
}