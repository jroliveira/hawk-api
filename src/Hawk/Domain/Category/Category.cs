namespace Hawk.Domain.Category
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Category : Entity<string>
    {
        private Category(in string name, in uint transactions)
            : base(name.ToPascalCase()) => this.Transactions = transactions;

        public uint Transactions { get; }

        public static Try<Category> NewCategory(in Option<string> nameOption, in Option<uint> transactionsOption = default) =>
            nameOption
                ? new Category(
                    nameOption.Get(),
                    transactionsOption.GetOrElse(default))
                : Failure<Category>(new InvalidObjectException("Invalid category."));
    }
}
