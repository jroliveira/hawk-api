namespace Hawk.Domain.Account.Commands
{
    using System;

    using Hawk.Domain.Shared.Commands;

    public interface IUpsertAccount : ICommand<UpsertParam<Guid, Account>>
    {
    }
}
