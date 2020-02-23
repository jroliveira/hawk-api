namespace Hawk.Domain.PaymentMethod.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IUpsertPaymentMethod : ICommand<UpsertParam<string, PaymentMethod>>
    {
    }
}
