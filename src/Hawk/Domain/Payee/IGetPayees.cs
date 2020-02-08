namespace Hawk.Domain.Payee
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    public interface IGetPayees
    {
        Task<Try<Page<Try<Payee>>>> GetResult(Option<Email> email, Filter filter);
    }
}
