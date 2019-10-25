namespace Hawk.WebApi.Features.Account
{
    using FluentValidation;

    internal sealed class NewAccountModelValidator : AbstractValidator<NewAccountModel>
    {
        internal NewAccountModelValidator() => this.RuleFor(model => model.Email)
            .NotEmpty()
            .WithMessage("Account e-mail is required.");
    }
}
