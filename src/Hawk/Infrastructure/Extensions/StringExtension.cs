namespace Hawk.Infrastructure.Extensions
{
    using Hawk.Infrastructure.Monad;

    using static System.Enum;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public static class StringExtension
    {
        public static Option<TEnum> ToEnum<TEnum>(this string @this)
            where TEnum : struct => TryParse(@this, out TEnum result)
                ? Some(result)
                : None();
    }
}
