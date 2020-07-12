namespace Hawk.Domain.Transaction
{
    using System;
    using System.Linq;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Shared.Money;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Payment : IEquatable<Option<Payment>>
    {
        private Payment(
            in Money cost,
            in DateTime date,
            in PaymentMethod paymentMethod)
        {
            this.Cost = cost;
            this.Date = date;
            this.PaymentMethod = paymentMethod;
        }

        public Money Cost { get; }

        public DateTime Date { get; }

        public PaymentMethod PaymentMethod { get; }

        public static Try<Payment> NewPayment(
            in Option<Money> costOption,
            in Option<DateTime> dateOption,
            in Option<PaymentMethod> paymentMethodOption) =>
                costOption
                && dateOption
                && paymentMethodOption
                    ? new Payment(costOption.Get(), dateOption.Get(), paymentMethodOption.Get())
                    : Failure<Payment>(new InvalidObjectException("Invalid payment."));

        public bool Equals(Option<Payment> otherOption) => otherOption
            .Fold(false)(other => this.Cost.Equals(other.Cost)
                                  && this.Date.Equals(other.Date)
                                  && this.PaymentMethod.Equals(other.PaymentMethod));
    }
}
