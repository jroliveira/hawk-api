namespace Hawk.Domain.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertPaymentMethod
    {
        Task<Try<PaymentMethod>> Execute(Email email, string name, Option<PaymentMethod> entity);
    }
}
