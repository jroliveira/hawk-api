namespace Hawk.Domain.Category.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IUpsertCategory : ICommand<UpsertParam<string, Category>>
    {
    }
}
