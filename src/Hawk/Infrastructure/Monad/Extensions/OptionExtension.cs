namespace Hawk.Infrastructure.Monad.Extensions
{
    public static class OptionExtension
    {
        public static TValue GetOrElse<TValue>(this Option<TValue> @this, TValue @default) => @this.Match(
            value => value,
            () => @default);

        public static string? GetStringOrElse<TValue>(this Option<TValue> @this, string @default) => @this.Match(
            value => value?.ToString(),
            () => @default);
    }
}
