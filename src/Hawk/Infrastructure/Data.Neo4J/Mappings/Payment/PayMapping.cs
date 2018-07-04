namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using System;
    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure.Monad;
    using static Hawk.Domain.Entities.Payment.Pay;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class PayMapping
    {
        private const string Year = "year";
        private const string Month = "month";
        private const string Day = "day";
        private const string Method = "method";

        public static Try<Pay> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => PriceMapping.MapFrom(record).Match(
                _ => _,
                price => MethodMapping.MapFrom(record.GetRecord(Method)).Match(
                    _ => _,
                    method => CreateWith(
                        price,
                        Date(
                            record.Get<int>(Year),
                            record.Get<int>(Month),
                            record.Get<int>(Day)),
                        method.Method))),
            () => new NullReferenceException("Pay cannot be null."));
    }
}