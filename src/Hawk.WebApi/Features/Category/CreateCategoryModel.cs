namespace Hawk.WebApi.Features.Category
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Category;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Category.Category;

    public class CreateCategoryModel
    {
        public CreateCategoryModel(string name) => this.Name = name;

        [Required]
        public string Name { get; }

        public static implicit operator Option<Category>(CreateCategoryModel model) => NewCategory(model.Name);
    }
}
