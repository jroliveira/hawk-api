namespace Hawk.Domain.Transaction
{
    using System;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

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

        public static Try<Payment> CreateWith(Option<Price> priceOption, in Option<DateTime> dateOption, Option<PaymentMethod> paymentMethodOption)
        {
            var price = priceOption.GetOrElse(default);
            if (price == null)
            {
                return new ArgumentNullException(nameof(price), "Payment price cannot be null.");
            }

            var paymentMethod = paymentMethodOption.GetOrElse(default);
            if (paymentMethod == null)
            {
                return new ArgumentNullException(nameof(paymentMethodOption), "Payment method cannot be null.");
            }

            return dateOption.Match<Try<Payment>>(
                date => new Payment(price, date, paymentMethod),
                () => new ArgumentNullException(nameof(dateOption), "Payment date cannot be null."));
        }
    }
}
