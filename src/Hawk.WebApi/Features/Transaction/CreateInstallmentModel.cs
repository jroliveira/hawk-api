namespace Hawk.WebApi.Features.Transaction
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Installment;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Domain.Installment.Installment;
    using static Hawk.Domain.Installment.Installments;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class CreateInstallmentModel
    {
        public CreateInstallmentModel(
            string frequency,
            uint installments)
        {
            this.Frequency = frequency;
            this.Installments = installments;
        }

        [Required]
        public string Frequency { get; }

        [Required]
        public uint Installments { get; }

        public static implicit operator Option<Installment>(CreateInstallmentModel model) => model == null
            ? None()
            : NewInstallment(
                model.Frequency.ToEnum<InstallmentFrequency>(),
                NewInstallments(model.Installments)).ToOption();
    }
}
