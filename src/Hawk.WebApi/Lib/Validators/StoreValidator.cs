namespace Hawk.WebApi.Lib.Validators
{
    using FluentValidation;

    using Hawk.WebApi.Models.Store.Post;

    internal sealed class StoreValidator : AbstractValidator<Store>
    {
        internal StoreValidator() => this.RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Nome da loja deve ser informado.");
    }
}
