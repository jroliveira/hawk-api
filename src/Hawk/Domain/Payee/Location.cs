namespace Hawk.Domain.Payee
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Location
    {
        private Location(in IEnumerable<Coordinate> coordinates) => this.Coordinates = coordinates.ToList();

        public IReadOnlyCollection<Coordinate> Coordinates { get; }

        public static Try<Location> NewLocation(
            in Option<IEnumerable<Option<Coordinate>>> coordinatesOption) =>
                coordinatesOption.Get().All(_ => _)
                    ? new Location(
                        coordinatesOption.Get().Select(tag => tag.Get()))
                    : Failure<Location>(new InvalidObjectException("Invalid coordinate."));
    }
}
