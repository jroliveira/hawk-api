namespace Hawk.WebApi.Features.PaymentMethod
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.PaymentMethod.PaymentMethod;

    public class CreatePaymentMethodModel
    {
        public CreatePaymentMethodModel(string name) => this.Name = name;

        [Required]
        public string Name { get; }

        public static implicit operator Option<PaymentMethod>(CreatePaymentMethodModel model) => NewPaymentMethod(model.Name);
    }
}
