namespace Hawk.Infrastructure.Filter
{
    using Http.Query.Filter;

    internal interface IWhere<out TReturn, in TFilter>
        where TFilter : Filter
    {
        TReturn Apply(TFilter filter, string node);
    }
}