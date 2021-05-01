namespace Hawk.Domain.Installment.Data.Neo4J
{
    using System.Linq;

    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Installment.Installments;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class InstallmentsMapping
    {
        internal static Try<Installments> MapInstallments(Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Installments>(NotFound(nameof(Installments))))(record => NewInstallments(
                record.Get<uint>("total"),
                record.Get<uint>("current")));
    }
}
