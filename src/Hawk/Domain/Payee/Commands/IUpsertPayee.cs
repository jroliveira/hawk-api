namespace Hawk.Domain.Payee.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IUpsertPayee : ICommand<UpsertParam<string, Payee>>
    {
    }
}
