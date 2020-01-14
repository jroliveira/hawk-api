namespace Hawk.WebApi.Features.Tag
{
    using FluentValidation;

    internal sealed class NewTagModelValidator : AbstractValidator<NewTagModel>
    {
        internal NewTagModelValidator() => this.RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Tag name is required.");
    }
}
