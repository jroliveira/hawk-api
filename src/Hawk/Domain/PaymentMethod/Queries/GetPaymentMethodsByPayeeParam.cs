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
            in Email email,
            in Payee payee,
            in Filter filter)
            : base(email)
        {
            this.Payee = payee;
            this.Filter = filter;
        }

        public Payee Payee { get; }

        public Filter Filter { get; }

        public static Try<GetPaymentMethodsByPayeeParam> NewGetPaymentMethodsByPayeeParam(
            in Option<Email> emailOption,
            in Option<Payee> payeeOption,
            in Filter filter) =>
                emailOption
                && payeeOption
                    ? new GetPaymentMethodsByPayeeParam(
                        emailOption.Get(),
                        payeeOption.Get(),
                        filter)
                    : Failure<GetPaymentMethodsByPayeeParam>(new InvalidObjectException("Invalid get payment methods by payee param."));
    }
}
