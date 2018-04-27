namespace System.Linq
{
    using Hawk.Infrastructure.Monad;

    public static partial class LinqExtension
    {
        public static Try<TReturn> Select<TSuccess, TReturn>(this Try<TSuccess> @this, Func<TSuccess, TReturn> selector) => @this.Map(selector);

        public static Try<TReturn> SelectMany<TSource, TReturn>(this Try<Try<TSource>> @this, Func<TSource, TReturn> selector) => @this.Match(
            failure => new Try<TReturn>(failure),
            success => success.Match(
                failure => failure,
                collection => new Try<TReturn>(selector(collection))));

        public static Try<TReturn> SelectMany<TSource, TCollection, TReturn>(this Try<TSource> @this, Func<TSource, Try<TCollection>> collectionSelector, Func<TCollection, TReturn> selector) => @this.Match(
            failure => failure,
            success => @this.Bind(collectionSelector).Match(
                failure => failure,
                collection => new Try<TReturn>(selector(collection))));
    }
}
