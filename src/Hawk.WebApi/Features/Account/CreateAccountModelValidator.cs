namespace Hawk.WebApi.Features.Account
{
    using FluentValidation;

    internal sealed class CreateAccountModelValidator : AbstractValidator<CreateAccountModel>
    {
        internal CreateAccountModelValidator() => this.RuleFor(model => model.Email)
            .NotEmpty()
            .WithMessage("Account e-mail is required.");
    }
}
