namespace Hawk.Infrastructure.Monad.Extensions
{
    public static class EitherExtension
    {
        public static TRight GetOrElse<TLeft, TRight>(this Either<TLeft, TRight> @this, TRight @default) => @this.Match(
            _ => @default,
            value => value);
    }
}