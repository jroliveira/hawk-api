namespace Hawk.Domain.Configuration.Queries
{
    using Hawk.Domain.Shared.Queries;

    public interface IGetConfigurationByDescription : IQuery<GetByIdParam<string>, Configuration>
    {
    }
}
