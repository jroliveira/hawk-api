namespace Hawk.WebApi.Lib.Validators
{
    using FluentValidation;

    using Hawk.WebApi.Models.Account.Post;

    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            this.RuleFor(model => model.Email)
                .NotEmpty()
                .WithMessage("E-mail deve ser informado.");

            this.RuleFor(model => model.ConfirmPassword)
                .Equal(model => model.Password)
                .WithMessage("Confirmação de senha deve ser igual a senha.");
        }
    }
}
