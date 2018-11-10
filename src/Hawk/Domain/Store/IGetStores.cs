﻿namespace Hawk.Domain.Store
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    public interface IGetStores
    {
        Task<Try<Paged<Try<(Store Store, uint Count)>>>> GetResult(string email, Filter filter);
    }
}
