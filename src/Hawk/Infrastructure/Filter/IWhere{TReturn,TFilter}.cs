namespace Hawk.Infrastructure.Filter
{
    using Http.Query.Filter;

    internal interface IWhere<out TReturn, in TFilter>
        where TFilter : IFilter
    {
        string Name { get; }

        TReturn Apply(TFilter filter);
    }
}
