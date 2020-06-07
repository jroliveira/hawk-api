namespace Hawk.WebApi.Features.PaymentMethod
{
    using FluentValidation;

    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;

    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    internal sealed class CreatePaymentMethodModelValidator : AbstractValidator<CreatePaymentMethodModel>
    {
        internal CreatePaymentMethodModelValidator(
            Email email,
            string paymentMethod,
            IGetPaymentMethodByName getPaymentMethodByName) => this.RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessage("Payment method name is required.")
                .MustAsync(async (name, _) => await getPaymentMethodByName.GetResult(NewGetByIdParam(email, paymentMethod)) || paymentMethod.Equals(name))
                .WithMessage("Path payment method must be equal to body payment method.");
    }
}
