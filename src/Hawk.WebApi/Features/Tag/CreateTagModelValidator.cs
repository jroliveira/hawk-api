namespace Hawk.WebApi.Features.Tag
{
    using FluentValidation;

    internal sealed class CreateTagModelValidator : AbstractValidator<CreateTagModel>
    {
        internal CreateTagModelValidator() => this.RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Tag name is required.");
    }
}
