namespace Hawk.Domain.Category
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Category : ValueObject<Category, string>
    {
        private Category(string name, uint transactions)
            : base(name) => this.Transactions = transactions;

        public uint Transactions { get; }

        public static Try<Category> NewCategory(
            Option<string> name,
            Option<uint> transactions = default) =>
                name
                ? new Category(name.Get(), transactions.GetOrElse(default))
                : Failure<Category>(new InvalidObjectException("Invalid category."));
    }
}
