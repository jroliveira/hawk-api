namespace Hawk.Domain.Shared.Queries
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class GetAllParam : Param
    {
        private GetAllParam(
            Email email,
            Filter filter)
            : base(email) => this.Filter = filter;

        public Filter Filter { get; }

        public static Try<GetAllParam> NewGetByAllParam(
            Option<Email> email,
            Filter filter) => email
                ? new GetAllParam(
                    email.Get(),
                    filter)
                : Failure<GetAllParam>(new InvalidObjectException("Invalid get all param."));
    }
}
