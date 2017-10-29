namespace Hawk.WebApi.Lib.Validators
{
    using FluentValidation;

    using Hawk.WebApi.Models.Transaction;

    public class ParcelValidator : AbstractValidator<Parcel>
    {
        public ParcelValidator()
        {
            this.RuleFor(model => model.Number)
                .GreaterThan(0)
                .WithMessage("N�mero da parcela deve ser informado.")
                .LessThanOrEqualTo(model => model.Total)
                .WithMessage("N�mero da parcela deve ser menor ou igual ao total de parcelas.");

            this.RuleFor(model => model.Total)
                .GreaterThan(0)
                .WithMessage("Total de parcelas deve ser informado.");
        }
    }
}