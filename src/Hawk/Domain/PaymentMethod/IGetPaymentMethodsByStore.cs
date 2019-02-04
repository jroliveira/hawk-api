namespace Hawk.Domain.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    public interface IGetPaymentMethodsByStore
    {
        Task<Try<Paged<Try<(PaymentMethod PaymentMethod, uint Count)>>>> GetResult(Email email, string store, Filter filter);
    }
}
