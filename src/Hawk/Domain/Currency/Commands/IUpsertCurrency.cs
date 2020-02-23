namespace Hawk.Domain.Currency.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IUpsertCurrency : ICommand<UpsertParam<string, Currency>>
    {
    }
}
