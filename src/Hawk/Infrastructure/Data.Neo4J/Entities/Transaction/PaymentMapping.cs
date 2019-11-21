namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Transaction.Payment;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod.PaymentMethodMapping;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    using static PriceMapping;

    internal static class PaymentMapping
    {
        private const string Year = "year";
        private const string Month = "month";
        private const string Day = "day";
        private const string Method = "method";

        internal static Try<Payment> MapPayment(Option<Neo4JRecord> record) => record.Match(
            some => NewPayment(
                MapPrice(record),
                Date(
                    some.Get<int>(Year),
                    some.Get<int>(Month),
                    some.Get<int>(Day)),
                MapPaymentMethod(some.GetRecord(Method))),
            () => new NotFoundException("Payment not found."));
    }
}
