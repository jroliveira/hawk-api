namespace System.Linq
{
    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;

    internal static partial class LinqExtension
    {
        internal static TryModel<TReturn> Select<TModel, TReturn>(this TryModel<TModel> @this, Func<TModel, TReturn> selector) => @this.Match<TryModel<TReturn>>(
            _ => _,
            success => selector(success));
    }
}
