namespace Hawk.Domain.Category
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertCategory
    {
        Task<Try<Category>> Execute(Email email, string name, Option<Category> entity);
    }
}
