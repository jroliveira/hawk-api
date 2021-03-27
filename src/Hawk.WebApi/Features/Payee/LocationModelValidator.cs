namespace Hawk.WebApi.Features.Payee
{
    using System.Linq;

    using FluentValidation;

    internal sealed class LocationModelValidator : AbstractValidator<LocationModel?>
    {
        internal LocationModelValidator() =>
            this.When(model => model?.Coordinates?.Any() != null, () =>
                this.RuleForEach(model => model!.Coordinates)
                    .SetValidator(new CoordinateModelValidator()));
    }
}
