namespace Hawk.WebApi.GraphQl.Types
{
    using GraphQL.Types;

    using Hawk.WebApi.GraphQl.Sources;

    public class TransactionsType : ObjectGraphType<Transactions>
    {
        public TransactionsType()
        {
            this.Field(transaction => transaction.Amount);
            this.Field(transaction => transaction.Quantity);
            this.Field(transaction => transaction.Period);
            this.Field(transaction => transaction.Type);
        }
    }
}