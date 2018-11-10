namespace Hawk.WebApi.Models.Transaction
{
    using System;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;

    public sealed class Payment
    {
        public Payment(double value, DateTime date, string method, string currency)
        {
            this.Value = value;
            this.Date = date;
            this.Method = method;
            this.Currency = currency;
        }

        public double Value { get; }

        public DateTime Date { get; }

        public string Method { get; }

        public string Currency { get; }

        public static implicit operator Payment(Domain.Transaction.Payment entity) => new Payment(entity.Price.Value, entity.Date, entity.PaymentMethod, entity.Price.Currency);

        public static implicit operator Option<Domain.Transaction.Payment>(Payment model)
        {
            var currency = Domain.Currency.Currency.CreateWith(model.Currency);
            var price = Price.CreateWith(model.Value, currency);
            var paymentMethod = Domain.PaymentMethod.PaymentMethod.CreateWith(model.Method);

            return Domain.Transaction.Payment.CreateWith(price, model.Date, paymentMethod);
        }
    }
}
