namespace Hawk.Domain.Category.Data.Neo4J
{
    using System.Linq;

    using Hawk.Domain.Category;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Category.Category;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class CategoryMapping
    {
        internal static Try<Category> MapCategory(in IRecord data) => MapCategory(MapRecord(data, "data"));

        internal static Try<Category> MapCategory(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Category>(NotFound(nameof(Category))))(record => NewCategory(
                record.Get<string>("name"),
                record.Get<uint>("transactions")));
    }
}
