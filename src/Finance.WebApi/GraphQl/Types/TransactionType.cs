namespace Finance.WebApi.GraphQl.Types
{
    using Finance.WebApi.GraphQl.Sources;

    using GraphQL.Types;

    public class TransactionType : ObjectGraphType<Transaction>
    {
        public TransactionType()
        {
            this.Field(transaction => transaction.Id);
            this.Field(transaction => transaction.Type);
            this.Field(transaction => transaction.Store);
            this.Field<PaymentType>("payment");
            this.Field<ParcelType>("parcel");
            this.Field<ListGraphType<StringGraphType>>("tags");
        }
    }
}
