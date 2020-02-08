namespace Hawk.Domain.Category
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertCategory
    {
        Task<Try<Category>> Execute(Option<Email> email, Option<string> name, Option<Category> entity);
    }
}
