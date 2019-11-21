namespace Hawk.Domain.PaymentMethod
{
    using System;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class PaymentMethod : IEquatable<PaymentMethod>
    {
        private PaymentMethod(string name) => this.Name = name;

        public string Name { get; }

        public static implicit operator string(PaymentMethod paymentMethod) => paymentMethod.Name;

        public static Try<PaymentMethod> NewPaymentMethod(Option<string> name) =>
            name
            ? new PaymentMethod(name.Get())
            : Failure<PaymentMethod>(new InvalidObjectException("Invalid payment method."));

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is PaymentMethod paymentMethod && this.Equals(paymentMethod);
        }

        public bool Equals(PaymentMethod other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.Name, other.Name);
        }

        public override int GetHashCode() => this.Name != null ? this.Name.GetHashCode() : 0;
    }
}
