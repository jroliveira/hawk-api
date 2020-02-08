namespace Hawk.Domain.Currency
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetCurrencyByName
    {
        Task<Try<Currency>> GetResult(Option<Email> email, Option<string> name);
    }
}
