namespace Hawk.Domain.Category.Queries
{
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    public interface IGetCategories : IQuery<GetAllParam, Page<Try<Category>>>
    {
    }
}
