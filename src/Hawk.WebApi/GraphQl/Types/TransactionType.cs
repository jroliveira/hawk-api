namespace Hawk.WebApi.GraphQl.Types
{
    using GraphQL.Types;

    using Hawk.WebApi.GraphQl.Sources;

    public class TransactionType : ObjectGraphType<Transaction>
    {
        public TransactionType()
        {
            this.Field(transaction => transaction.Id);
            this.Field(transaction => transaction.Type);
            this.Field<StoreType>("store");
            this.Field<PaymentType>("payment");
            this.Field<ParcelType>("parcel");
            this.Field<ListGraphType<StringGraphType>>("tags");
        }
    }
}
