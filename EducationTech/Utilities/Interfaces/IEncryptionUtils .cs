using EducationTech.Utilities.Abstract;

namespace EducationTech.Utilities.Interfaces
{
    public interface IEncryptionUtils : IUltils
    {
        string HashPassword(string password, out byte[] salt);

        bool VerifyPassword(string password, string hashedPassword, byte[] salt);
    }
}
