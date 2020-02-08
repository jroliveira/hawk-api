namespace Hawk.Domain.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    public interface IGetTags
    {
        Task<Try<Page<Try<Tag>>>> GetResult(Option<Email> email, Filter filter);
    }
}
