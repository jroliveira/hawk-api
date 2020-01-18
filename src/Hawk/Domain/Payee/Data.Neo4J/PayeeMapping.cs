namespace Hawk.Domain.Payee.Data.Neo4J
{
    using Hawk.Domain.Payee;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Payee.Payee;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;

    internal static class PayeeMapping
    {
        internal static Try<Payee> MapPayee(IRecord data) => MapPayee(MapRecord(data, "data"));

        internal static Try<Payee> MapPayee(Option<Neo4JRecord> record) => record.Match(
            some => NewPayee(some.Get<string>("name"), some.Get<uint>("transactions")),
            () => new NotFoundException("Payee not found."));
    }
}
