namespace Hawk.Domain.Category
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetCategoryByName
    {
        Task<Try<Category>> GetResult(Email email, string name);
    }
}
