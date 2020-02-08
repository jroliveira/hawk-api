namespace Hawk.Domain.Payee
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertPayee
    {
        Task<Try<Payee>> Execute(Option<Email> email, Option<Payee> entity);

        Task<Try<Payee>> Execute(Option<Email> email, Option<string> name, Option<Payee> entity);
    }
}
