using EducationTech.Shared.Utilities.Abstract;
using Microsoft.AspNetCore.Http;

namespace EducationTech.Shared.Utilities.Interfaces
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
