﻿namespace Hawk.Domain.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    public interface IGetStores
    {
        Task<Try<Page<Try<Store>>>> GetResult(Email email, Filter filter);
    }
}
