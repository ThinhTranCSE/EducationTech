using EducationTech.Shared.Utilities.Abstract;

namespace EducationTech.Shared.Utilities.Interfaces
{
    public interface IEncryptionUtils : IUtils
    {
        string HashPassword(string password, out byte[] salt);

        bool VerifyPassword(string password, string hashedPassword, byte[] salt);
    }
}
