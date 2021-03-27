namespace Hawk.Domain.Payee
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Coordinate
    {
        private Coordinate(
            in double latitude,
            in double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude { get; }

        public double Longitude { get; }

        public static Try<Coordinate> NewCoordinate(
            in Option<double> latitudeOption,
            in Option<double> longitudeOption) =>
                latitudeOption
                && longitudeOption
                    ? new Coordinate(
                        latitudeOption.Get(),
                        longitudeOption.Get())
                    : Failure<Coordinate>(new InvalidObjectException("Invalid coordinate."));
    }
}
