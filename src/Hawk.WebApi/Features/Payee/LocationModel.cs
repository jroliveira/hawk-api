namespace Hawk.WebApi.Features.Payee
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Payee;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Domain.Payee.Coordinate;

    using static Hawk.Domain.Payee.Location;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class LocationModel
    {
        public LocationModel(IEnumerable<CoordinateModel> coordinates) => this.Coordinates = coordinates;

        public IEnumerable<CoordinateModel> Coordinates { get; }

        public static implicit operator LocationModel?(in Option<Location> entityOption) => entityOption
            .Fold(default(LocationModel))(entity => new LocationModel(entity.Coordinates.Select(coordinate => new CoordinateModel(
                coordinate.Latitude,
                coordinate.Longitude))));

        public static implicit operator Option<Location>(in LocationModel? model) => model == null
            ? None()
            : NewLocation(
                Some(model.Coordinates.Select(coordinate => NewCoordinate(
                    coordinate.Latitude,
                    coordinate.Longitude).ToOption()))).ToOption();
    }
}
