namespace Hawk.WebApi.Features.Tag
{
    using FluentValidation;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag.Queries;

    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    internal sealed class CreateTagModelValidator : AbstractValidator<CreateTagModel>
    {
        internal CreateTagModelValidator(
            Email email,
            string tag,
            IGetTagByName getTagByName) => this.RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessage("Tag name is required.")
                .MustAsync(async (name, _) => await getTagByName.GetResult(NewGetByIdParam(email, tag)) || tag.Equals(name))
                .WithMessage("Path tag must be equal to body tag.");
    }
}
