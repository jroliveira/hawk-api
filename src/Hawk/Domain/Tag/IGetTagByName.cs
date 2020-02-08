namespace Hawk.Domain.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetTagByName
    {
        Task<Try<Tag>> GetResult(Option<Email> email, Option<string> name);
    }
}
