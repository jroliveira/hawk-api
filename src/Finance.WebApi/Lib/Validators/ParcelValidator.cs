namespace Finance.WebApi.Lib.Validators
{
    using Finance.WebApi.Models.Transaction;

    using FluentValidation;

    public class ParcelValidator : AbstractValidator<Parcel>
    {
        public ParcelValidator()
        {
            this.RuleFor(model => model.Number)
                .GreaterThan(0)
                .WithMessage("Número da parcela deve ser informado.")
                .LessThanOrEqualTo(model => model.Total)
                .WithMessage("Número da parcela deve ser menor ou igual ao total de parcelas.");

            this.RuleFor(model => model.Total)
                .GreaterThan(0)
                .WithMessage("Total de parcelas deve ser informado.");
        }
    }
}