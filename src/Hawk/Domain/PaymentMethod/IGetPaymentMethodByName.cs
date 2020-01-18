namespace Hawk.Domain.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetPaymentMethodByName
    {
        Task<Try<PaymentMethod>> GetResult(Email email, string name);
    }
}
