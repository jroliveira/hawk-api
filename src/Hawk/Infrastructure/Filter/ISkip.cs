namespace Hawk.Infrastructure.Filter
{
    using Http.Query.Filter;

    public interface ISkip<out TReturn, in TFilter>
        where TFilter : Filter
    {
        TReturn Apply(TFilter filter);
    }
}
