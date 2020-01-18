namespace Hawk.Domain.Payee
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetPayeeByName
    {
        Task<Try<Payee>> GetResult(Email email, string name);
    }
}
