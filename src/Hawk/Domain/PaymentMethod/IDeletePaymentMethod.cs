namespace Hawk.Domain.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IDeletePaymentMethod
    {
        Task<Try<Unit>> Execute(Option<Email> email, Option<string> name);
    }
}
