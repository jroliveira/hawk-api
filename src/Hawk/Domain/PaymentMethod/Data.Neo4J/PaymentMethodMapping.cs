namespace Hawk.Domain.PaymentMethod.Data.Neo4J
{
    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.PaymentMethod.PaymentMethod;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;

    internal static class PaymentMethodMapping
    {
        internal static Try<PaymentMethod> MapPaymentMethod(IRecord data) => MapPaymentMethod(MapRecord(data, "data"));

        internal static Try<PaymentMethod> MapPaymentMethod(Option<Neo4JRecord> record) => record.Match(
            some => NewPaymentMethod(some.Get<string>("name"), some.Get<uint>("transactions")),
            () => new NotFoundException("Payment method not found."));
    }
}
