namespace Hawk.Domain.Transaction
{
    using System;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Shared.Money;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Payment : IEquatable<Option<Payment>>
    {
        private Payment(Money cost, DateTime date, PaymentMethod paymentMethod)
        {
            this.Cost = cost;
            this.Date = date;
            this.PaymentMethod = paymentMethod;
        }

        public Money Cost { get; }

        public DateTime Date { get; }

        public PaymentMethod PaymentMethod { get; }

        public static Try<Payment> NewPayment(
            Option<Money> cost,
            in Option<DateTime> date,
            Option<PaymentMethod> paymentMethod) =>
                cost
                && date
                && paymentMethod
                ? new Payment(cost.Get(), date.Get(), paymentMethod.Get())
                : Failure<Payment>(new InvalidObjectException("Invalid payment."));

        public bool Equals(Option<Payment> other) => other.Match(
            some => this.Cost.Equals(some.Cost)
                    && this.Date.Equals(some.Date)
                    && this.PaymentMethod.Equals(some.PaymentMethod),
            () => false);
    }
}
