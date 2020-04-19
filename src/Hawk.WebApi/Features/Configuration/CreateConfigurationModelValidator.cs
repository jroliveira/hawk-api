namespace Hawk.WebApi.Features.Configuration
{
    using FluentValidation;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;
    using Hawk.WebApi.Features.Shared.Money;

    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;

    internal sealed class CreateConfigurationModelValidator : AbstractValidator<CreateConfigurationModel>
    {
        internal CreateConfigurationModelValidator(
            Email email,
            IGetCategoryByName getCategoryByName,
            IGetCurrencyByCode getCurrencyByCode,
            IGetPayeeByName getPayeeByName,
            IGetPaymentMethodByName getPaymentMethodByName)
        {
            this.RuleFor(model => model.Type)
                .NotEmpty()
                .WithMessage("Type is required.");

            this.RuleFor(model => model.PaymentMethod)
                .NotEmpty()
                .WithMessage("Payment method is required.")
                .MustAsync(async (method, _) => await getPaymentMethodByName.GetResult(NewGetByIdParam(email, method)))
                .WithMessage("Payment method not found.");

            this.RuleFor(model => model.Currency)
                .NotNull()
                .WithMessage("Currency is required.")
                .SetValidator(new CurrencyModelValidator(email, getCurrencyByCode));

            this.RuleFor(model => model.Payee)
                .NotEmpty()
                .WithMessage("Payee is required.")
                .MustAsync(async (payee, _) => await getPayeeByName.GetResult(NewGetByIdParam(email, payee)))
                .WithMessage("Payee not found.");

            this.RuleFor(model => model.Category)
                .NotEmpty()
                .WithMessage("Category is required.")
                .MustAsync(async (category, _) => await getCategoryByName.GetResult(NewGetByIdParam(email, category)))
                .WithMessage("Category not found.");

            this.RuleFor(model => model.Tags)
                .NotNull()
                .NotEmpty()
                .WithMessage("Must be at least one tag for configuration.");
        }
    }
}
