namespace Hawk.Domain.Tag.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IUpsertTag : ICommand<UpsertParam<string, Tag>>
    {
    }
}
