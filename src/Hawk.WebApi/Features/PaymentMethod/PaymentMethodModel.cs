namespace Hawk.WebApi.Features.PaymentMethod
{
    using Hawk.Domain.PaymentMethod;

    public sealed class PaymentMethodModel
    {
        private PaymentMethodModel(PaymentMethod entity)
        {
            this.Name = entity.Id;
            this.Transactions = entity.Transactions;
        }

        public string Name { get; }

        public uint Transactions { get; }

        internal static PaymentMethodModel NewPaymentMethodModel(in PaymentMethod entity) => new PaymentMethodModel(entity);
    }
}
