namespace Finance.WebApi.GraphQl.Types
{
    using Finance.WebApi.GraphQl.Sources;

    using GraphQL.Types;

    public class PaymentType : ObjectGraphType<Payment>
    {
        public PaymentType()
        {
            this.Field(payment => payment.Value);
            this.Field(payment => payment.Date);
            this.Field(payment => payment.Method);
            this.Field(payment => payment.Currency);
        }
    }
}