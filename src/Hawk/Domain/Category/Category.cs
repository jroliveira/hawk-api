namespace Hawk.Domain.Category
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Domain.Category.Icon;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Category : Entity<string>
    {
        private Category(
            in string name,
            in Icon icon,
            in uint transactions)
            : base(name.ToPascalCase())
        {
            this.Transactions = transactions;
            this.Icon = icon;
        }

        public Icon Icon { get; set; }

        public uint Transactions { get; }

        public static Try<Category> NewCategory(in Option<string> nameOption) => NewCategory(
            nameOption,
            NewIcon("undefined"));

        public static Try<Category> NewCategory(
            in Option<string> nameOption,
            in Option<Icon> iconOption,
            in Option<uint> transactionsOption = default) =>
                nameOption
                && iconOption
                    ? new Category(
                        nameOption.Get(),
                        iconOption.Get(),
                        transactionsOption.GetOrElse(default))
                    : Failure<Category>(new InvalidObjectException("Invalid category."));
    }
}
