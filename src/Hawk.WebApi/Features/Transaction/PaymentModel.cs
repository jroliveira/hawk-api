namespace Hawk.WebApi.Features.Transaction
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;

    public sealed class PaymentModel
    {
        public PaymentModel(double value, DateTime date, string method, string currency)
        {
            this.Value = value;
            this.Date = date;
            this.Method = method;
            this.Currency = currency;
        }

        [Required]
        public double Value { get; }

        [DataType(DataType.Date)]
        public DateTime Date { get; }

        [Required]
        public string Method { get; }

        [Required]
        public string Currency { get; }

        public static implicit operator PaymentModel(Payment entity) => new PaymentModel(entity.Price.Value, entity.Date, entity.PaymentMethod, entity.Price.Currency);

        public static implicit operator Option<Payment>(PaymentModel model)
        {
            var currency = Domain.Currency.Currency.CreateWith(model.Currency);
            var price = Price.CreateWith(model.Value, currency);
            var paymentMethod = Domain.PaymentMethod.PaymentMethod.CreateWith(model.Method);

            return Payment.CreateWith(price, model.Date, paymentMethod);
        }
    }
}
