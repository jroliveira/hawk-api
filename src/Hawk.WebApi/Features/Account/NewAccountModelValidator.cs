namespace Hawk.WebApi.Features.Account
{
    using FluentValidation;

    internal sealed class NewAccountModelValidator : AbstractValidator<NewAccountModel>
    {
        internal NewAccountModelValidator()
        {
            this.RuleFor(model => model.Email)
                .NotEmpty()
                .WithMessage("Account e-mail is required.");

            this.RuleFor(model => model.Password)
                .NotEmpty()
                .WithMessage("Account password is required.");

            this.RuleFor(model => model.ConfirmPassword)
                .Equal(model => model.Password)
                .WithMessage("Account confirm password must match account password.");
        }
    }
}
