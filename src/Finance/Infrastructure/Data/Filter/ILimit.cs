namespace Finance.Infrastructure.Data.Filter
{
    using Http.Query.Filter;

    public interface ILimit<out TReturn, in TFilter>
        where TFilter : Filter
    {
        TReturn Apply(TFilter filter);
    }
}