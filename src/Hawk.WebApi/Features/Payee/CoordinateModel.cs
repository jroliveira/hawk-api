namespace Hawk.WebApi.Features.Payee
{
    using Hawk.Domain.Payee;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Payee.Coordinate;

    public sealed class CoordinateModel
    {
        public CoordinateModel(
            double latitude,
            double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude { get; }

        public double Longitude { get; }

        public static implicit operator CoordinateModel(in Coordinate entity) => new CoordinateModel(
            entity.Latitude,
            entity.Longitude);

        public static implicit operator Option<Coordinate>(in CoordinateModel model) => NewCoordinate(
            model.Latitude,
            model.Longitude);
    }
}
