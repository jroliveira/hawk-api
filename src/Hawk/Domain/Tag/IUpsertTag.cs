namespace Hawk.Domain.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertTag
    {
        Task<Try<Tag>> Execute(Email email, string name, Option<Tag> entity);
    }
}
