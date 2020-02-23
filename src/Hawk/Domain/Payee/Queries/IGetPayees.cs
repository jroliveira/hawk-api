namespace Hawk.Domain.Payee.Queries
{
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    public interface IGetPayees : IQuery<GetAllParam, Page<Try<Payee>>>
    {
    }
}
