namespace Hawk.WebApi.Features.Payee
{
    using FluentValidation;

    internal sealed class CoordinateModelValidator : AbstractValidator<CoordinateModel>
    {
        internal CoordinateModelValidator()
        {
            this.RuleFor(model => model.Latitude)
                .NotNull()
                .WithMessage("Latitude is required.");

            this.RuleFor(model => model.Longitude)
                .NotNull()
                .WithMessage("Longitude is required.");
        }
    }
}
