namespace Hawk.Domain.Configuration
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertConfiguration
    {
        Task<Try<Configuration>> Execute(Email email, Option<Configuration> entity);
    }
}
