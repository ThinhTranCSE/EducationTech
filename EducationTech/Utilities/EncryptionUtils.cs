using EducationTech.Enums;
using EducationTech.Utilities.Interfaces;
using Org.BouncyCastle.Tls;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace EducationTech.Utilities
{
    public class EncryptionUtils : IEncryptionUtils
    {
        private readonly IConfiguration _configuration;

        private int _keySize { get; set; }
        private int _iterations { get; set; }
        private HashAlgorithmName _hashAlgorithm { get; set; }


        public EncryptionUtils(IConfiguration configuration)
        {
            _configuration = configuration;
            _keySize = _configuration.GetValue<int>("Encryption:KeySize");
            _iterations = _configuration.GetValue<int>("Encryption:Iterations");
            
            string algorithm = _configuration.GetValue<string>("Encryption:HashAlgorithm");
            _hashAlgorithm = new HashAlgorithmName(algorithm);
        }

        public string HashPassword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(_keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                _iterations,
                _hashAlgorithm,
                _keySize
             );
            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string hashedPassword, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _hashAlgorithm, _keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hashedPassword));
        }
    }
}
