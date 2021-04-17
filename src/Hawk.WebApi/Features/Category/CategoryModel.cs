namespace Hawk.WebApi.Features.Category
{
    using System.Linq;

    using Hawk.Domain.Category;
    using Hawk.Infrastructure.Monad;

    using static System.String;

    using static Hawk.Domain.Category.Category;
    using static Hawk.Domain.Category.Icon;

    public sealed class CategoryModel
    {
        public CategoryModel(string name)
            : this(name, Empty, default)
        {
        }

        private CategoryModel(
            string name,
            string icon,
            uint transactions)
        {
            this.Name = name;
            this.Icon = icon;
            this.Transactions = transactions;
        }

        public string Name { get; }

        public string Icon { get; }

        public uint Transactions { get; }

        public static implicit operator Option<Category>(in CategoryModel model) => NewCategory(
            model.Name,
            NewIcon(model.Icon));

        public static implicit operator CategoryModel(in Category entity) => new CategoryModel(
            entity.Id,
            entity.Icon,
            entity.Transactions);

        internal static CategoryModel NewCategoryModel(in Category entity) => new CategoryModel(
            entity.Id,
            entity.Icon,
            entity.Transactions);
    }
}
