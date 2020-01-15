namespace Hawk.Domain.PaymentMethod
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class PaymentMethod : ValueObject<PaymentMethod, string>
    {
        private PaymentMethod(string name)
            : base(name)
        {
        }

        public static Try<PaymentMethod> NewPaymentMethod(Option<string> name) =>
            name
            ? new PaymentMethod(name.Get())
            : Failure<PaymentMethod>(new InvalidObjectException("Invalid payment method."));
    }
}
