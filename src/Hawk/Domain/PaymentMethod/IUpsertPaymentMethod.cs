namespace Hawk.Domain.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertPaymentMethod
    {
        Task<Try<PaymentMethod>> Execute(Option<Email> email, Option<PaymentMethod> entity);

        Task<Try<PaymentMethod>> Execute(Option<Email> email, Option<string> name, Option<PaymentMethod> entity);
    }
}
