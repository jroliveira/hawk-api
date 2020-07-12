namespace Hawk.Domain.PaymentMethod.Data.Neo4J
{
    using System.Linq;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.PaymentMethod.PaymentMethod;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class PaymentMethodMapping
    {
        internal static Try<PaymentMethod> MapPaymentMethod(in IRecord data) => MapPaymentMethod(MapRecord(data, "data"));

        internal static Try<PaymentMethod> MapPaymentMethod(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<PaymentMethod>(NotFound("Payment method")))(record => NewPaymentMethod(
                record.Get<string>("name"),
                record.Get<uint>("transactions")));
    }
}
