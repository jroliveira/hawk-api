namespace Hawk.Infrastructure.Monad.Utils
{
    using System;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.Monad;

    public static partial class Util
    {
        public static Try<DateTime> Date(Option<int> year, Option<int> month, Option<int> day) =>
            year
            && month
            && day
            ? new DateTime(year.Get(), month.Get(), day.Get())
            : Failure<DateTime>(new InvalidObjectException("Invalid date."));
    }
}
