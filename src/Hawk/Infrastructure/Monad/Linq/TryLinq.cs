namespace System.Linq
{
    using Hawk.Infrastructure.Monad;

    public static partial class LinqExtension
    {
        public static Try<TReturn> Select<TModel, TReturn>(this Try<TModel> @this, Func<TModel, TReturn> selector) => @this.Match<Try<TReturn>>(
            _ => _,
            success => selector(success));
    }
}
