namespace Hawk.Domain.Transaction.Data.Neo4J
{
    using System.Linq;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.PaymentMethod.Data.Neo4J.PaymentMethodMapping;
    using static Hawk.Domain.Shared.Money.Data.Neo4J.MoneyMapping;
    using static Hawk.Domain.Transaction.Payment;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class PaymentMapping
    {
        internal static Try<Payment> MapPayment(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Payment>(NotFound(nameof(Payment))))(record => NewPayment(
                MapMoney(record.GetRecord("cost")),
                Date(
                    record.Get<int>("year"),
                    record.Get<int>("month"),
                    record.Get<int>("day")),
                MapPaymentMethod(record.GetRecord("method"))));
    }
}
