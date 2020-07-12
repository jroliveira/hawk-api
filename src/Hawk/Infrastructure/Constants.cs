namespace Hawk.Infrastructure
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    public static class Constants
    {
        public static class ErrorMessages
        {
            public static NotFoundException NotFound(in string entity) => new NotFoundException($"{entity} not found.");

            public static NullObjectException IsRequired(in string @object) => new NullObjectException($"{@object} is required.");
        }
    }
}
