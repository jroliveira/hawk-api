namespace Hawk.Domain.Budget.Queries
{
    using System;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class GetBudgetByIdParam : Param
    {
        private GetBudgetByIdParam(
            in Email email,
            in Guid id,
            in Filter filter)
            : base(email)
        {
            this.Id = id;
            this.Filter = filter;
        }

        public Guid Id { get; }

        public Filter Filter { get; }

        public static Try<GetBudgetByIdParam> NewGetBudgetByIdParam(
            in Option<Email> emailOption,
            in Option<Guid> idOption,
            in Filter filter) =>
                emailOption
                && idOption
                    ? new GetBudgetByIdParam(
                        emailOption.Get(),
                        idOption.Get(),
                        filter)
                    : Failure<GetBudgetByIdParam>(new InvalidObjectException("Invalid get budget by id param."));
    }
}
