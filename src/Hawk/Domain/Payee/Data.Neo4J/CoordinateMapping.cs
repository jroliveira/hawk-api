namespace Hawk.Domain.Payee.Data.Neo4J
{
    using System.Linq;

    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Payee.Coordinate;

    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class CoordinateMapping
    {
        internal static Try<Coordinate> MapCoordinate(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Coordinate>(NotFound(nameof(Coordinate))))(record => NewCoordinate(
                record.Get<double>("latitude"),
                record.Get<double>("longitude")));
    }
}
