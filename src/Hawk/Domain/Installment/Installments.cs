namespace Hawk.Domain.Installment
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Installments
    {
        public Installments(
            in uint total,
            in uint current)
        {
            this.Total = total;
            this.Current = current;
        }

        public uint Total { get; }

        public uint Current { get; }

        public static Try<Installments> NewInstallments(
            in Option<uint> totalOption) => NewInstallments(
                totalOption,
                0u);

        public static Try<Installments> NewInstallments(
            in Option<uint> totalOption,
            in Option<uint> currentOption) =>
                totalOption
                && currentOption
                    ? new Installments(
                        totalOption.Get(),
                        currentOption.Get())
                    : Failure<Installments>(new InvalidObjectException("Invalid installments."));
    }
}
