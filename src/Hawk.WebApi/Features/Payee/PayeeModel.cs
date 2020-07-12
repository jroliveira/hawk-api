namespace Hawk.WebApi.Features.Payee
{
    using Hawk.Domain.Payee;

    public sealed class PayeeModel
    {
        private PayeeModel(Payee entity)
        {
            this.Name = entity.Id;
            this.Transactions = entity.Transactions;
        }

        public string Name { get; }

        public uint Transactions { get; }

        internal static PayeeModel NewPayeeModel(in Payee entity) => new PayeeModel(entity);
    }
}
