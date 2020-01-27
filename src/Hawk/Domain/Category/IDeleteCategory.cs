namespace Hawk.Domain.Category
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IDeleteCategory
    {
        Task<Try<Unit>> Execute(Email email, string name);
    }
}
