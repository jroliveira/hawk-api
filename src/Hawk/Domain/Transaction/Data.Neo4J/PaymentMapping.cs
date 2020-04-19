namespace Hawk.Domain.Transaction.Data.Neo4J
{
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.PaymentMethod.Data.Neo4J.PaymentMethodMapping;
    using static Hawk.Domain.Shared.Money.Data.Neo4J.MoneyMapping;
    using static Hawk.Domain.Transaction.Payment;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class PaymentMapping
    {
        internal static Try<Payment> MapPayment(Option<Neo4JRecord> record) => record.Match(
            some => NewPayment(
                MapMoney(some.GetRecord("cost")),
                Date(
                    some.Get<int>("year"),
                    some.Get<int>("month"),
                    some.Get<int>("day")),
                MapPaymentMethod(some.GetRecord("method"))),
            () => new NotFoundException("Payment not found."));
    }
}
