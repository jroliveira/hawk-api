namespace Hawk.WebApi.Infrastructure.Hal.Page
{
    using System.Collections.Generic;

    internal interface IPage<out TClass>
        where TClass : class
    {
        string Name { get; }

        IReadOnlyCollection<TClass> Data { get; }

        int Skip { get; }

        int Limit { get; }

        long Pages { get; }
    }
}
