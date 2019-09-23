namespace Hawk.WebApi.Infrastructure.ErrorHandling.TryModel
{
    using System;

    using Hawk.WebApi.Infrastructure.ErrorHandling.ErrorModels;

    public interface ITryModel<out TModel>
    {
        TModel Get();

        TReturn Match<TReturn>(Func<ErrorModel, TReturn> failure, Func<TModel, TReturn> success);
    }
}
