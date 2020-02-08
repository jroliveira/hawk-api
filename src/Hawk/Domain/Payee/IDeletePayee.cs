namespace Hawk.Domain.Payee
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IDeletePayee
    {
        Task<Try<Unit>> Execute(Option<Email> email, Option<string> name);
    }
}
