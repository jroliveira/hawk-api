namespace Hawk.Domain.Payee.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IDeletePayee : ICommand<DeleteParam<string>>
    {
    }
}
