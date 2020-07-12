namespace Hawk.Domain.Tag.Queries
{
    using Hawk.Domain.Payee;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public class GetTagsByPayeeParam : Param
    {
        private GetTagsByPayeeParam(
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

        public static Try<GetTagsByPayeeParam> NewGetTagsByPayeeParam(
            in Option<Email> emailOption,
            in Option<Payee> payeeOption,
            in Filter filter) =>
                emailOption
                && payeeOption
                    ? new GetTagsByPayeeParam(
                        emailOption.Get(),
                        payeeOption.Get(),
                        filter)
                    : Failure<GetTagsByPayeeParam>(new InvalidObjectException("Invalid get tags by payee param."));
    }
}
