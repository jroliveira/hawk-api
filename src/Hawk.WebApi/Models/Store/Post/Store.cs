namespace Hawk.WebApi.Models.Store.Post
{
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    public sealed class Store
    {
        public string Name { get; set; }

        public static implicit operator Option<Domain.Store.Store>(Store model) => Domain.Store.Store.CreateWith(model.Name);

        public static implicit operator Domain.Store.Store(Store model) => Domain.Store.Store.CreateWith(model.Name).GetOrElse(default);
    }
}
