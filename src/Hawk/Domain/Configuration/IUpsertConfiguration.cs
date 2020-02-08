namespace Hawk.Domain.Configuration
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertConfiguration
    {
        Task<Try<Configuration>> Execute(Option<Email> email, Option<string> description, Option<Configuration> entity);
    }
}
