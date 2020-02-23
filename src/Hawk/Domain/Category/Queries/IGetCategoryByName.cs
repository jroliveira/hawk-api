namespace Hawk.Domain.Category.Queries
{
    using Hawk.Domain.Shared.Queries;

    public interface IGetCategoryByName : IQuery<GetByIdParam<string>, Category>
    {
    }
}
