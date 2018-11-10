namespace Hawk.Infrastructure.Monad.Utils
{
    using System;

    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    public static partial class Util
    {
        private static readonly DateTime DefaultDate = default;

        public static DateTime Date(Option<int> yearOption, Option<int> monthOption, Option<int> dayOption) => new DateTime(
            yearOption.GetOrElse(DefaultDate.Year),
            monthOption.GetOrElse(DefaultDate.Month),
            dayOption.GetOrElse(DefaultDate.Day));
    }
}
