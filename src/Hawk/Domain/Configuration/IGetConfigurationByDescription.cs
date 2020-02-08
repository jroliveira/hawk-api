namespace Hawk.Domain.Configuration
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetConfigurationByDescription
    {
        Task<Try<Configuration>> GetResult(Option<Email> email, Option<string> description);
    }
}
