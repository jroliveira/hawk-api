namespace Hawk.WebApi.Features.Transaction
{
    using FluentValidation;

    using static System.Convert;

    internal sealed class CreateInstallmentModelValidator : AbstractValidator<CreateInstallmentModel>
    {
        internal CreateInstallmentModelValidator()
        {
            this.RuleFor(model => model.Frequency)
                .NotEmpty()
                .WithMessage("Installment frequency is required.");

            this.RuleFor(model => model.Installments)
                .GreaterThan(ToUInt32(1))
                .WithMessage("Number of installments should be greater than 1.");
        }
    }
}
