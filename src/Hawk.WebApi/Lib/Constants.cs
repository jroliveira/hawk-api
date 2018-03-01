namespace Hawk.WebApi.Lib
{
    public static class Constants
    {
        public static class Exceptions
        {
            public static string NotFound => "NotFoundException";

            public static string UnauthorizedAccess => "UnauthorizedAccessException";

            public static string Validation => "ValidationException";
        }

        public static class Api
        {
            public static string Cors => "CorsPolicy";
        }
    }
}
