namespace Hawk.Infrastructure.Pagination
{
    using System.Collections.Generic;

    public interface IPage<out T> : IPage
    {
        IReadOnlyCollection<T> Data { get; }
    }
}
