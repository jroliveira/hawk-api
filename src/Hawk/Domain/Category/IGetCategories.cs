namespace Hawk.Domain.Category
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    public interface IGetCategories
    {
        Task<Try<Page<Try<Category>>>> GetResult(Email email, Filter filter);
    }
}
