namespace Hawk.Domain.Budget.Queries
{
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    public interface IGetBudgets : IQuery<GetAllParam, Page<Try<Budget>>>
    {
    }
}
