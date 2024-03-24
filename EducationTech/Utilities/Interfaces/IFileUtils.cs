using EducationTech.Utilities.Abstract;

namespace EducationTech.Utilities.Interfaces
{
    public interface IFileUtils : IUtils
    {
        Task<string> SaveFileAsync(string filePath, Stream fileStream);
        Task<string> SaveFileAsync(string filePath, byte[] fileBytes);
        Task<string> SaveFileAsync(string filePath, IFormFile file);

        Task<byte[]> GetFileContentAsync(string filePath);

        Task<bool> DeleteFileAsync(string filePath);

    }
}
