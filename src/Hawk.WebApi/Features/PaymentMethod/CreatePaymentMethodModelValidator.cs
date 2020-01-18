namespace Hawk.WebApi.Features.PaymentMethod
{
    using FluentValidation;

    internal sealed class CreatePaymentMethodModelValidator : AbstractValidator<CreatePaymentMethodModel>
    {
        internal CreatePaymentMethodModelValidator() => this.RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Payment method name is required.");
    }
}
