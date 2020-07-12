namespace Hawk.Domain.Shared.Queries
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    public interface IQuery<TParam, TReturn>
        where TParam : Param
    {
        Task<Try<TReturn>> GetResult(Option<TParam> paramOption);
    }
}
