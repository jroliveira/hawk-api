namespace Hawk.WebApi.Features.Budget
{
    using System;

    using FluentValidation;

    internal sealed class RecurrenceModelValidator : AbstractValidator<RecurrenceModel>
    {
        internal RecurrenceModelValidator()
        {
            this.RuleFor(model => model.Start)
                .NotEqual(default(DateTime))
                .WithMessage("Recurrence start is required.");

            this.RuleFor(model => model.Frequency)
                .NotEmpty()
                .WithMessage("Budget frequency is required.");
        }
    }
}
