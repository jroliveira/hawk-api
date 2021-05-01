namespace Hawk.Domain.Installment.Data.Neo4J
{
    using System;
    using System.Linq;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Installment.Data.Neo4J.InstallmentsMapping;
    using static Hawk.Domain.Installment.Installment;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class InstallmentMapping
    {
        internal static Try<Installment> MapInstallment(in IRecord data) => MapInstallment(MapRecord(data, "data"));

        internal static Try<Installment> MapInstallment(Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Installment>(NotFound(nameof(Installment))))(record => NewInstallment(
                record.Get<Guid>("id"),
                record
                    .Get<string>("frequency")
                    .Select(some => some.ToEnum<InstallmentFrequency>()),
                MapInstallments(record.GetRecord("installments"))));
    }
}
