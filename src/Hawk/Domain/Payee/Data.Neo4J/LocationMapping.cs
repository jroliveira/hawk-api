namespace Hawk.Domain.Payee.Data.Neo4J
{
    using System.Linq;

    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Domain.Payee.Data.Neo4J.CoordinateMapping;
    using static Hawk.Domain.Payee.Location;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class LocationMapping
    {
        internal static Try<Location> MapLocation(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Location>(NotFound(nameof(Location))))(record => NewLocation(
                Some(record.GetListOfNeo4JRecord("coordinates").Select(coordinate => MapCoordinate(coordinate).ToOption()))));
    }
}
