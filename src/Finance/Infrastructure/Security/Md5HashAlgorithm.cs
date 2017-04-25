namespace Finance.Infrastructure.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class Md5HashAlgorithm : IHashAlgorithm
    {
        public string Hash(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            var utf8Encoding = new UTF8Encoding();
            var bytes = utf8Encoding.GetBytes(text);
            HashAlgorithm algorithm = new HMACMD5();

            if (algorithm == null)
            {
                throw new NullReferenceException("HashAlgorithm Md5 is not null.");
            }

            var hash = algorithm.ComputeHash(bytes);

            return BitConverter.ToString(hash)
                .Replace("-", string.Empty)
                .ToLower();
        }
    }
}