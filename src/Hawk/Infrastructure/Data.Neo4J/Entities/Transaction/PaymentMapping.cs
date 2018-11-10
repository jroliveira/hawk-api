namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;

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

        internal static Try<Payment> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => PriceMapping.MapFrom(record).Match(
                _ => _,
                price => PaymentMethodMapping.MapFrom(record.GetRecord(Method)).Match(
                    _ => _,
                    paymentMethod => CreateWith(
                        price,
                        Date(
                            record.Get<int>(Year),
                            record.Get<int>(Month),
                            record.Get<int>(Day)),
                        paymentMethod.PaymentMethod))),
            () => new NullReferenceException("Payment cannot be null."));
    }
}
