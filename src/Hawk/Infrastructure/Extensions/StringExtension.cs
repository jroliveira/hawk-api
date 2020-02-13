namespace Hawk.Infrastructure.Extensions
{
    using System.Globalization;

    using Hawk.Infrastructure.Monad;

    using static System.Enum;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public static class StringExtension
    {
        public static Option<TEnum> ToEnum<TEnum>(this string @this)
            where TEnum : struct => TryParse(@this, out TEnum result)
                ? Some(result)
                : None();

        public static string ToKebabCase(this string @this) => @this
            .ToLowerCase()
            .Replace(' ', '-');

        public static string ToPascalCase(this string @this) => new CultureInfo("en-US", false)
            .TextInfo
            .ToTitleCase(@this)
            .Trim();

        public static string ToLowerCase(this string @this) => @this
            .Trim()
            .ToLower();

        public static string ToUpperCase(this string @this) => @this
            .Trim()
            .ToUpper();
    }
}
