namespace Hawk.Domain.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertTag
    {
        Task<Try<Tag>> Execute(Option<Email> email, Option<Tag> entity);

        Task<Try<Tag>> Execute(Option<Email> email, Option<string> name, Option<Tag> entity);
    }
}
