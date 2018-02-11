namespace Hawk.Infrastructure
{
    using System;

    public static class Guard
    {
        public static T NotNull<T>(T parameter, string paramName, string message)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(paramName, message);
            }

            return parameter;
        }

        public static string NotNullNorEmpty(string parameter, string paramName, string message)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentException(message, paramName);
            }

            return parameter;
        }
    }
}
