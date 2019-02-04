namespace Hawk.Domain.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    public interface IGetTags
    {
        Task<Try<Paged<Try<(Tag Tag, uint Count)>>>> GetResult(Email email, Filter filter);
    }
}
