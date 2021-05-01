namespace Hawk.Domain.Installment.Queries
{
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    public interface IGetInstallments : IQuery<GetAllParam, Page<Try<Installment>>>
    {
    }
}
