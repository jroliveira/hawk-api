namespace Finance.Infrastructure.Filter
{
    using Http.Query.Filter;

    public interface IWhere<out TReturn, in TFilter>
        where TFilter : Filter
    {
        TReturn Apply(TFilter filter, string node);
    }
}