namespace Hawk.WebApi.Features.Payee
{
    using Hawk.Domain.Payee;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Payee.Payee;

    public sealed class PayeeModel
    {
        public PayeeModel(string name)
            : this(name, default, default)
        {
        }

        private PayeeModel(
            string name,
            LocationModel? location,
            uint transactions)
        {
            this.Name = name;
            this.Location = location;
            this.Transactions = transactions;
        }

        public string Name { get; }

        public LocationModel? Location { get; }

        public uint Transactions { get; }

        public static implicit operator Option<Payee>(in PayeeModel model) => NewPayee(
            model.Name,
            model.Location);

        public static implicit operator PayeeModel(in Payee entity) => new PayeeModel(
            entity.Id,
            entity.LocationOption,
            entity.Transactions);

        internal static PayeeModel NewPayeeModel(in Payee entity) => new PayeeModel(
            entity.Id,
            entity.LocationOption,
            entity.Transactions);
    }
}
