﻿namespace Hawk.Domain.Queries.PaymentMethod
{
    using System.Threading.Tasks;
    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;
    using Http.Query.Filter;

    public interface IGetAllByStoreQuery
    {
        Task<Try<Paged<Try<(Method Method, uint Count)>>>> GetResult(string email, string store, Filter filter);
    }
}
