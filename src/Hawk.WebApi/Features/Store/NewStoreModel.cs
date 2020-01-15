namespace Hawk.WebApi.Features.Store
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Store.Store;

    public class NewStoreModel
    {
        public NewStoreModel(string name) => this.Name = name;

        [Required]
        public string Name { get; }

        public static implicit operator Option<Store>(NewStoreModel model) => MapNewStore(model);

        public static implicit operator NewStoreModel(Store entity) => new NewStoreModel(entity);

        public static Option<Store> MapNewStore(NewStoreModel model) => NewStore(model.Name);
    }
}
