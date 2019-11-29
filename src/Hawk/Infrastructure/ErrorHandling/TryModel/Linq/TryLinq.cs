namespace System.Linq
{
    using Hawk.Infrastructure.ErrorHandling.TryModel;

    public static partial class LinqExtension
    {
        public static TryModel<TReturn> Select<TModel, TReturn>(this TryModel<TModel> @this, Func<TModel, TReturn> selector) => @this.Match<TryModel<TReturn>>(
            _ => _,
            success => selector(success));
    }
}
