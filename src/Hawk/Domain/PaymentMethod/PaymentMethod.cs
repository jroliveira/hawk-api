namespace Hawk.Domain.PaymentMethod
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class PaymentMethod : Entity<string>
    {
        private PaymentMethod(in string name, in uint transactions)
            : base(name.ToPascalCase()) => this.Transactions = transactions;

        public uint Transactions { get; }

        public static Try<PaymentMethod> NewPaymentMethod(in Option<string> nameOption, in Option<uint> transactionsOption = default) =>
            nameOption
                ? new PaymentMethod(nameOption.Get(), transactionsOption.GetOrElse(default))
                : Failure<PaymentMethod>(new InvalidObjectException("Invalid payment method."));
    }
}
