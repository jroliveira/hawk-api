namespace Hawk.Domain.Category.Data.Neo4J
{
    using Hawk.Domain.Category;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Category.Category;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;

    internal static class CategoryMapping
    {
        internal static Try<Category> MapCategory(IRecord data) => MapCategory(MapRecord(data, "data"));

        internal static Try<Category> MapCategory(Option<Neo4JRecord> record) => record.Match(
            some => NewCategory(some.Get<string>("name"), some.Get<uint>("transactions")),
            () => new NotFoundException("Category not found."));
    }
}
