namespace Hawk.Domain.Currency
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertCurrency
    {
        Task<Try<Currency>> Execute(Email email, Option<Currency> entity);

        Task<Try<Currency>> Execute(Email email, string name, Option<Currency> entity);
    }
}
