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

        public static implicit operator Parcel(Domain.Entities.Parcel entity)
        {
            Parcel model = default;
            if (entity != null)
            {
                model = new Parcel(entity.Number, entity.Total);
            }

            return model;
        }

        public static implicit operator Option<Domain.Entities.Parcel>(Parcel model)
        {
            Try<Domain.Entities.Parcel> entity = default;
            if (model != null)
            {
                entity = Domain.Entities.Parcel.CreateWith(model.Total, model.Number);
            }

            return entity;
        }
    }
}
