namespace Hawk.Domain.Transaction.Commands
{
    using System;

    using Hawk.Domain.Shared.Commands;

    public interface IUpsertTransaction : ICommand<UpsertParam<Guid, Transaction>>
    {
    }
}
