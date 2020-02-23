namespace Hawk.Domain.Tag.Queries
{
    using Hawk.Domain.Shared.Queries;

    public interface IGetTagByName : IQuery<GetByIdParam<string>, Tag>
    {
    }
}
