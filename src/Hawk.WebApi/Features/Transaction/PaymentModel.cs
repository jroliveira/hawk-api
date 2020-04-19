namespace Hawk.WebApi.Features.Transaction
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Features.Shared.Money;

    using static Hawk.Domain.PaymentMethod.PaymentMethod;
    using static Hawk.Domain.Transaction.Payment;

    public sealed class PaymentModel
    {
        public PaymentModel(
            MoneyModel cost,
            DateTime date,
            string method)
        {
            this.Cost = cost;
            this.Date = date;
            this.Method = method;
        }

        [Required]
        public MoneyModel Cost { get; }

        [DataType(DataType.Date)]
        public DateTime Date { get; }

        [Required]
        public string Method { get; }

        public static implicit operator PaymentModel(Payment entity) => new PaymentModel(
            entity.Cost,
            entity.Date,
            entity.PaymentMethod);

        public static implicit operator Option<Payment>(PaymentModel model) => NewPayment(
            model.Cost,
            model.Date,
            NewPaymentMethod(model.Method));
    }
}
