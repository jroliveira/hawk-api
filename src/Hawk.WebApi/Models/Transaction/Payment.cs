namespace Hawk.WebApi.Models.Transaction
{
    using System;

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

        public static implicit operator Payment(Domain.Entities.Payment.Pay entity) => new Payment(entity.Price.Value, entity.Date, entity.Method, entity.Price.Currency);

        public static implicit operator Option<Domain.Entities.Payment.Pay>(Payment model)
        {
            var currency = Domain.Entities.Payment.Currency.CreateWith(model.Currency);
            var price = Domain.Entities.Payment.Price.CreateWith(model.Value, currency);
            var method = Domain.Entities.Payment.Method.CreateWith(model.Method);

            return Domain.Entities.Payment.Pay.CreateWith(price, model.Date, method);
        }
    }
}