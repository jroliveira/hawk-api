namespace Hawk.Domain.Budget.Queries
{
    using Hawk.Domain.Shared.Queries;

    public interface IGetBudgetById : IQuery<GetBudgetByIdParam, Budget>
    {
    }
}
