namespace Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod
{
    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.PaymentMethod.PaymentMethod;

    internal static class PaymentMethodMapping
    {
        private const string Name = "name";
        private const string Total = "total";
        private const string Data = "data";

        internal static Try<(PaymentMethod PaymentMethod, uint Count)> MapFrom(IRecord data) => data.GetRecord(Data).Match(
            record =>
            {
                var total = record.Get<uint>(Total);
                if (!total.IsDefined)
                {
                    return new InvalidObjectException("Invalid payment method.");
                }

                return MapFrom(record).Match<Try<(PaymentMethod, uint)>>(
                    _ => _,
                    paymentMethod => (paymentMethod, total.Get()));
            },
            () => new NotFoundException("Payment method not found."));

        internal static Try<PaymentMethod> MapFrom(Option<Record> record) => record.Match(
            some => CreateWith(some.Get<string>(Name)),
            () => new NotFoundException("Payment method not found."));
    }
}
