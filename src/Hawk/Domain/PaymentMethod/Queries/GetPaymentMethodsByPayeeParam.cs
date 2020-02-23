namespace Hawk.Domain.PaymentMethod.Queries
{
    using Hawk.Domain.Payee;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public class GetPaymentMethodsByPayeeParam : Param
    {
        private GetPaymentMethodsByPayeeParam(
            Email email,
            Payee payee,
            Filter filter)
            : base(email)
        {
            this.Payee = payee;
            this.Filter = filter;
        }

        public Payee Payee { get; }

        public Filter Filter { get; }

        public static Try<GetPaymentMethodsByPayeeParam> NewGetPaymentMethodsByPayeeParam(
            Option<Email> email,
            Option<Payee> payee,
            Filter filter) =>
                email
                && payee
                    ? new GetPaymentMethodsByPayeeParam(
                        email.Get(),
                        payee.Get(),
                        filter)
                    : Failure<GetPaymentMethodsByPayeeParam>(new InvalidObjectException("Invalid get payment methods by payee param."));
    }
}
