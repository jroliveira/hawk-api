namespace Hawk.Domain.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    public interface IGetTagsByPayee
    {
        Task<Try<Page<Try<Tag>>>> GetResult(Email email, string payee, Filter filter);
    }
}
