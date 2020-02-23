namespace Hawk.Domain.Transaction.Queries
{
    using System;

    using Hawk.Domain.Shared.Queries;

    public interface IGetTransactionById : IQuery<GetByIdParam<Guid>, Transaction>
    {
    }
}
