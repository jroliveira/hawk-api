namespace Hawk.WebApi.Features.Installment
{
    using System.Linq;

    using Hawk.Domain.Installment;
    using Hawk.Infrastructure.Monad;

    public sealed class InstallmentModel
    {
        private InstallmentModel(
            string id,
            string frequency,
            InstallmentsModel installments)
        {
            this.Id = id;
            this.Frequency = frequency;
            this.Installments = installments;
        }

        public string Id { get; }

        public string Frequency { get; }

        public InstallmentsModel Installments { get; set; }

        public static implicit operator InstallmentModel?(Option<Installment> entityOption) => entityOption
            .Fold(default(InstallmentModel))(entity => new InstallmentModel(
                entity.Id.ToString(),
                entity.Frequency.ToString(),
                entity.Installments));

        internal static InstallmentModel NewInstallmentModel(in Installment entity) => new InstallmentModel(
            entity.Id.ToString(),
            entity.Frequency.ToString(),
            entity.Installments);
    }
}
