using System.Security.Cryptography;
using System.Text;

namespace FDRWebsite.Server.Authentication
{
    public class PasswordHashService
    {
        private const int KEY_SIZE = 64;
        private const int SALT_SIZE = 16;
        private const int ITERATIONS = 350000;
        private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public static string CreateHash(string email, string password)
        {
            var salt = email.Substring(0, Math.Min(email.Length, SALT_SIZE));
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                Encoding.UTF8.GetBytes(salt),
                ITERATIONS,
                hashAlgorithm,
                KEY_SIZE);

            return Convert.ToHexString(hash);
        }
    }
}
