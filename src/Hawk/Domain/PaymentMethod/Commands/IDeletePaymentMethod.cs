namespace Hawk.Domain.PaymentMethod.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IDeletePaymentMethod : ICommand<DeleteParam<string>>
    {
    }
}
