namespace Hawk.WebApi.Features.Payee
{
    using FluentValidation;

    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.Shared;

    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    internal sealed class CreatePayeeModelValidator : AbstractValidator<CreatePayeeModel>
    {
        internal CreatePayeeModelValidator(
            Email email,
            string payee,
            IGetPayeeByName getPayeeByName)
        {
            this.RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessage("Payee name is required.")
                .MustAsync(async (name, _) => await getPayeeByName.GetResult(NewGetByIdParam(email, payee)) || payee.Equals(name))
                .WithMessage("Path payee must be equal to body payee.");

            this.RuleFor(model => model.Location)
                .SetValidator(new LocationModelValidator());
        }
    }
}
