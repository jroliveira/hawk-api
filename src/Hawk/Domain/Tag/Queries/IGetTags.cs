namespace Hawk.Domain.Tag.Queries
{
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    public interface IGetTags : IQuery<GetAllParam, Page<Try<Tag>>>
    {
    }
}
