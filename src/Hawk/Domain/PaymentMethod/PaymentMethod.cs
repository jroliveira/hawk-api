namespace Hawk.Domain.PaymentMethod
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class PaymentMethod : ValueObject<PaymentMethod, string>
    {
        private PaymentMethod(string name, uint transactions)
            : base(name.ToPascalCase()) => this.Transactions = transactions;

        public uint Transactions { get; }

        public static Try<PaymentMethod> NewPaymentMethod(
            Option<string> name,
            Option<uint> transactions = default) =>
                name
                ? new PaymentMethod(name.Get(), transactions.GetOrElse(default))
                : Failure<PaymentMethod>(new InvalidObjectException("Invalid payment method."));
    }
}
