namespace Hawk.Domain.Payee.Data.Neo4J
{
    using System.Linq;

    using Hawk.Domain.Payee;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Payee.Data.Neo4J.LocationMapping;
    using static Hawk.Domain.Payee.Payee;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class PayeeMapping
    {
        internal static Try<Payee> MapPayee(in IRecord data) => MapPayee(MapRecord(data, "data"));

        internal static Try<Payee> MapPayee(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Payee>(NotFound(nameof(Payee))))(record => NewPayee(
                record.Get<string>("name"),
                MapLocation(record.GetRecord("location")),
                record.Get<uint>("transactions")));
    }
}
