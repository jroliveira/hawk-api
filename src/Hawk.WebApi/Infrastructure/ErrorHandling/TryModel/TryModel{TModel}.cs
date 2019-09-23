namespace Hawk.WebApi.Infrastructure.ErrorHandling.TryModel
{
    using System;

    using Hawk.WebApi.Infrastructure.ErrorHandling.ErrorModels;

    public sealed class TryModel<TModel> : ITryModel<TModel>
    {
        private readonly ErrorModel errorModel;
        private readonly TModel model;

        public TryModel(TModel model) => this.model = model;

        public TryModel(ErrorModel errorModel) => this.errorModel = errorModel;

        public static implicit operator TryModel<TModel>(ErrorModel errorModel) => new TryModel<TModel>(errorModel);

        public static implicit operator TryModel<TModel>(TModel model) => new TryModel<TModel>(model);

        public TModel Get() => this.model;

        public TReturn Match<TReturn>(Func<ErrorModel, TReturn> failure, Func<TModel, TReturn> success) => this.errorModel != null
            ? failure(this.errorModel)
            : success(this.model);
    }
}
