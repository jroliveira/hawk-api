namespace Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod
{
    using System;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.PaymentMethod.PaymentMethod;

    internal static class PaymentMethodMapping
    {
        private const string Name = "name";
        private const string Total = "total";
        private const string Data = "data";

        internal static Try<(PaymentMethod PaymentMethod, uint Count)> MapFrom(IRecord data) => MapFrom(data.GetRecord(Data));

        internal static Try<(PaymentMethod PaymentMethod, uint Count)> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => CreateWith(record.Get<string>(Name)).Match<Try<(PaymentMethod, uint)>>(
                _ => _,
                paymentMethod => (paymentMethod, record.Get<uint>(Total).GetOrElse(0u))),
            () => new NullReferenceException("Payment method cannot be null."));
    }
}
