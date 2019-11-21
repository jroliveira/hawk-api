namespace Hawk.Domain.Transaction
{
    using System;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Payment
    {
        private Payment(Price price, DateTime date, PaymentMethod paymentMethod)
        {
            this.Price = price;
            this.Date = date;
            this.PaymentMethod = paymentMethod;
        }

        public Price Price { get; }

        public DateTime Date { get; }

        public PaymentMethod PaymentMethod { get; }

        public static Try<Payment> NewPayment(Option<Price> price, in Option<DateTime> date, Option<PaymentMethod> paymentMethod) =>
            price
            && date
            && paymentMethod
            ? new Payment(price.Get(), date.Get(), paymentMethod.Get())
            : Failure<Payment>(new InvalidObjectException("Invalid payment."));
    }
}
