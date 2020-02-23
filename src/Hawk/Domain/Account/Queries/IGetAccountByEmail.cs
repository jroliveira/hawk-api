namespace Hawk.Domain.Account.Queries
{
    using Hawk.Domain.Shared.Queries;

    public interface IGetAccountByEmail : IQuery<GetAccountByEmailParam, Account>
    {
    }
}
