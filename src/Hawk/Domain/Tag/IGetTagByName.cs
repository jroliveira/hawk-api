namespace Hawk.Domain.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetTagByName
    {
        Task<Try<Tag>> GetResult(Email email, string name);
    }
}
