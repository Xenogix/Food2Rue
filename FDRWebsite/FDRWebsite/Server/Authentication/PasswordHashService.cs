using System.Security.Cryptography;
using System.Text;

namespace FDRWebsite.Server.Authentication
{
    public class PasswordHashService
    {
        private const int keySize = 64;
        private const int saltSize = 16;
        private const int iterations = 350000;
        private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public static string CreateHash(string email, string password)
        {
            var salt = email.Substring(0, Math.Min(email.Length, saltSize));
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                Encoding.UTF8.GetBytes(salt),
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }
    }
}
