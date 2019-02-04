namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Transaction.Payment;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class PaymentMapping
    {
        private const string Year = "year";
        private const string Month = "month";
        private const string Day = "day";
        private const string Method = "method";

        internal static Try<Payment> MapFrom(Option<Record> record) => record.Match(
            some => CreateWith(
                PriceMapping.MapFrom(record),
                Date(
                    some.Get<int>(Year),
                    some.Get<int>(Month),
                    some.Get<int>(Day)),
                PaymentMethodMapping.MapFrom(some.GetRecord(Method))),
            () => new NotFoundException("Payment not found."));
    }
}
