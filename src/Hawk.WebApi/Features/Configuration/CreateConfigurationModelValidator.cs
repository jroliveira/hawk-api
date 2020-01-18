namespace Hawk.WebApi.Features.Configuration
{
    using FluentValidation;

    internal sealed class CreateConfigurationModelValidator : AbstractValidator<CreateConfigurationModel>
    {
        internal CreateConfigurationModelValidator()
        {
            this.RuleFor(model => model.Type)
                .NotEmpty()
                .WithMessage("Configuration type is required.");

            this.RuleFor(model => model.PaymentMethod)
                .NotEmpty()
                .WithMessage("Configuration payment method is required.");

            this.RuleFor(model => model.Currency)
                .NotEmpty()
                .WithMessage("Configuration currency is required.");

            this.RuleFor(model => model.Payee)
                .NotEmpty()
                .WithMessage("Configuration payee is required.");

            this.RuleFor(model => model.Tags)
                .NotNull()
                .NotEmpty()
                .WithMessage("Must be at least one tag for configuration.");
        }
    }
}
