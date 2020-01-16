namespace Hawk.Infrastructure.Security
{
    using System;
    using System.Security.Cryptography;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.Security.Cryptography.MD5;
    using static System.String;
    using static System.Text.Encoding;

    public static class Md5HashAlgorithm
    {
        private static readonly MD5 Algorithm = Create();

        public static Try<string> ComputeHash(string text)
        {
            try
            {
                return BitConverter
                    .ToString(Algorithm.ComputeHash(UTF8.GetBytes(text)))
                    .Replace("-", Empty)
                    .ToLower();
            }
            catch (Exception exception)
            {
                return new InternalException(exception.Message, exception);
            }
        }
    }
}
