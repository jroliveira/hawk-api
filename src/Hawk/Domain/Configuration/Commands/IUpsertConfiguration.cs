namespace Hawk.Domain.Configuration.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IUpsertConfiguration : ICommand<UpsertParam<string, Configuration>>
    {
    }
}
