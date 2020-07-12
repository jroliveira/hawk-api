namespace Hawk.Domain.Payee.Queries
{
    using Hawk.Domain.Category;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public class GetPayeesByCategoryParam : Param
    {
        private GetPayeesByCategoryParam(
            in Email email,
            in Category category,
            in Filter filter)
            : base(email)
        {
            this.Category = category;
            this.Filter = filter;
        }

        public Category Category { get; }

        public Filter Filter { get; }

        public static Try<GetPayeesByCategoryParam> NewGetPayeesByCategoryParam(
            in Option<Email> emailOption,
            in Option<Category> categoryOption,
            in Filter filter) =>
                emailOption
                && categoryOption
                    ? new GetPayeesByCategoryParam(
                        emailOption.Get(),
                        categoryOption.Get(),
                        filter)
                    : Failure<GetPayeesByCategoryParam>(new InvalidObjectException("Invalid get payees by category param."));
    }
}
