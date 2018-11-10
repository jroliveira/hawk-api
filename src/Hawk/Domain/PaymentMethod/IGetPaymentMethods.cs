namespace Hawk.Domain.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    public interface IGetPaymentMethods
    {
        Task<Try<Paged<Try<(PaymentMethod PaymentMethod, uint Count)>>>> GetResult(string email, Filter filter);
    }
}
