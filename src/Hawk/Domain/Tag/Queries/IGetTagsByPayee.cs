namespace Hawk.Domain.Tag.Queries
{
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    public interface IGetTagsByPayee : IQuery<GetTagsByPayeeParam, Page<Try<Tag>>>
    {
    }
}
