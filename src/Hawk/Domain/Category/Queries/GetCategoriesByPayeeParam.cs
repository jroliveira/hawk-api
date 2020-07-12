namespace Hawk.Domain.Category.Queries
{
    using Hawk.Domain.Payee;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public class GetCategoriesByPayeeParam : Param
    {
        private GetCategoriesByPayeeParam(
            in Email email,
            in Payee payee,
            in Filter filter)
            : base(email)
        {
            this.Payee = payee;
            this.Filter = filter;
        }

        public Payee Payee { get; }

        public Filter Filter { get; }

        public static Try<GetCategoriesByPayeeParam> NewGetCategoriesByPayeeParam(
            in Option<Email> emailOption,
            in Option<Payee> payeeOption,
            in Filter filter) =>
                emailOption
                && payeeOption
                    ? new GetCategoriesByPayeeParam(
                        emailOption.Get(),
                        payeeOption.Get(),
                        filter)
                    : Failure<GetCategoriesByPayeeParam>(new InvalidObjectException("Invalid get categories by payee param."));
    }
}
