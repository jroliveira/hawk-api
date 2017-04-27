namespace Finance.Infrastructure.Data.Filter
{
    using Http.Query.Filter;

    public interface IWhere<out TReturn, in TFilter, in TParam>
        where TFilter : Filter
    {
        TReturn Apply(TFilter filter, TParam param);
    }
}