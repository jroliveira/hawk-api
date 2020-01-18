namespace Hawk.Domain.Payee
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertPayee
    {
        Task<Try<Payee>> Execute(Email email, string name, Option<Payee> entity);
    }
}
