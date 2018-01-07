namespace Hawk.Infrastructure.Filter
{
    using Http.Query.Filter;

    internal interface ILimit<out TReturn, in TFilter>
        where TFilter : Filter
    {
        TReturn Apply(TFilter filter);
    }
}