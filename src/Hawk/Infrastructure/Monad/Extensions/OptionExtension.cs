namespace Hawk.Infrastructure.Monad.Extensions
{
    using System.Linq;

    public static class OptionExtension
    {
        public static TValue GetOrElse<TValue>(this in Option<TValue> @this, in TValue @default) => @this
            .Fold(@default)(_ => _);

        public static string? GetStringOrElse<TValue>(this in Option<TValue> @this, string? @default) => @this
            .Fold(@default)(value => value?.ToString());
    }
}
