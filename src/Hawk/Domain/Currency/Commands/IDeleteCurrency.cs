namespace Hawk.Domain.Currency.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IDeleteCurrency : ICommand<DeleteParam<string>>
    {
    }
}
