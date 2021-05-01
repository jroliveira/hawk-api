namespace Hawk.WebApi.Features.Installment
{
    using Hawk.Domain.Installment;

    public class InstallmentsModel
    {
        private InstallmentsModel(
            uint total,
            uint current)
        {
            this.Total = total;
            this.Current = current;
        }

        public uint Total { get; }

        public uint Current { get; }

        public static implicit operator InstallmentsModel(Installments entity) => new InstallmentsModel(
            entity.Total,
            entity.Current);
    }
}
