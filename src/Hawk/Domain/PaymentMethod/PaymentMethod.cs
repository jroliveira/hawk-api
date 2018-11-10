namespace Hawk.Domain.PaymentMethod
{
    using System;

    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.String;

    public sealed class PaymentMethod : IEquatable<PaymentMethod>
    {
        private PaymentMethod(string name) => this.Name = name;

        public string Name { get; }

        public static implicit operator string(PaymentMethod paymentMethod) => paymentMethod.Name;

        public static Try<PaymentMethod> CreateWith(Option<string> nameOption)
        {
            var name = nameOption.GetOrElse(Empty);

            if (IsNullOrEmpty(name))
            {
                return new ArgumentNullException(nameof(name), "Payment paymentMethod's name cannot be null or empty.");
            }

            return new PaymentMethod(name);
        }

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
