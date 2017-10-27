namespace Hawk.WebApi.GraphQl.Types
{
    using GraphQL.Types;

    using Hawk.WebApi.GraphQl.Sources;

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