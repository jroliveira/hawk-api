namespace Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod
{
    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.PaymentMethod.PaymentMethod;

    using static Neo4JRecord;

    internal static class PaymentMethodMapping
    {
        private const string Name = "name";
        private const string Total = "total";

        internal static Try<(PaymentMethod PaymentMethod, uint Count)> MapPaymentMethod(IRecord data) => MapRecord(data, "data").Match(
            record =>
            {
                var total = record.Get<uint>(Total);
                if (!total.IsDefined)
                {
                    return new InvalidObjectException("Invalid payment method.");
                }

                return MapPaymentMethod(record).Match<Try<(PaymentMethod, uint)>>(
                    _ => _,
                    paymentMethod => (paymentMethod, total.Get()));
            },
            () => new NotFoundException("Payment method not found."));

        internal static Try<PaymentMethod> MapPaymentMethod(Option<Neo4JRecord> record) => record.Match(
            some => NewPaymentMethod(some.Get<string>(Name)),
            () => new NotFoundException("Payment method not found."));
    }
}
