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
            Email email,
            Category category,
            Filter filter)
            : base(email)
        {
            this.Category = category;
            this.Filter = filter;
        }

        public Category Category { get; }

        public Filter Filter { get; }

        public static Try<GetPayeesByCategoryParam> NewGetPayeesByCategoryParam(
            Option<Email> email,
            Option<Category> category,
            Filter filter) =>
                email
                && category
                    ? new GetPayeesByCategoryParam(
                        email.Get(),
                        category.Get(),
                        filter)
                    : Failure<GetPayeesByCategoryParam>(new InvalidObjectException("Invalid get payees by category param."));
    }
}
