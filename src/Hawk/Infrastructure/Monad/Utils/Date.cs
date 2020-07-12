namespace Hawk.Infrastructure.Monad.Utils
{
    using System;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    public static partial class Util
    {
        public static Try<DateTime> Date(
            in Option<int> yearOption,
            in Option<int> monthOption,
            in Option<int> dayOption) =>
                yearOption
                && monthOption
                && dayOption
                    ? new DateTime(yearOption.Get(), monthOption.Get(), dayOption.Get())
                    : Failure<DateTime>(new InvalidObjectException("Invalid date."));
    }
}
