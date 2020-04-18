namespace Hawk.Domain.Configuration.Queries
{
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    public interface IGetConfigurations : IQuery<GetAllParam, Page<Try<Configuration>>>
    {
    }
}
