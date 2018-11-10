namespace Hawk.WebApi.Models.Transaction
{
    using Hawk.Infrastructure.Monad;

    public sealed class Parcel
    {
        public Parcel(uint number, uint total)
        {
            this.Number = number;
            this.Total = total;
        }

        public uint Number { get; }

        public uint Total { get; }

        public static implicit operator Parcel(Domain.Transaction.Parcel entity)
        {
            Parcel model = default;
            if (entity != null)
            {
                model = new Parcel(entity.Number, entity.Total);
            }

            return model;
        }

        public static implicit operator Option<Domain.Transaction.Parcel>(Parcel model)
        {
            Try<Domain.Transaction.Parcel> entity = default;
            if (model != null)
            {
                entity = Domain.Transaction.Parcel.CreateWith(model.Total, model.Number);
            }

            return entity;
        }
    }
}
