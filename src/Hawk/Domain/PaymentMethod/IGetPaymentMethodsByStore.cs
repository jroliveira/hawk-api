﻿namespace Hawk.Domain.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    public interface IGetPaymentMethodsByPayee
    {
        Task<Try<Page<Try<PaymentMethod>>>> GetResult(Email email, string payee, Filter filter);
    }
}
