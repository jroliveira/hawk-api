namespace Hawk.Infrastructure.ErrorHandling.TryModel
{
    using System;

    using Hawk.Infrastructure.ErrorHandling.ErrorModels;

    public interface ITryModel<out TModel>
    {
        TModel Get();

        TReturn Match<TReturn>(Func<ErrorModel, TReturn> failure, Func<TModel, TReturn> success);
    }
}
