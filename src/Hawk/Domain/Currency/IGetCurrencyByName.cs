namespace Hawk.Domain.Currency
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetCurrencyByName
    {
        Task<Try<Currency>> GetResult(Email email, string name);
    }
}
