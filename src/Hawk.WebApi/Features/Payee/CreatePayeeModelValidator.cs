namespace Hawk.WebApi.Features.Payee
{
    using FluentValidation;

    internal sealed class CreatePayeeModelValidator : AbstractValidator<CreatePayeeModel>
    {
        internal CreatePayeeModelValidator() => this.RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Payee name is required.");
    }
}
