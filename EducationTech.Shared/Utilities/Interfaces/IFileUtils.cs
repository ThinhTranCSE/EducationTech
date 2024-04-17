using EducationTech.Shared.DataStructures;
using EducationTech.Shared.Utilities.Abstract;
using Microsoft.AspNetCore.Http;
using MimeDetective.Engine;
using System.Collections.Immutable;

namespace EducationTech.Shared.Utilities.Interfaces
{
    public interface IFileUtils : IUtils
    {
        Task<string> SaveFile(string filePath, Stream fileStream);
        Task<string> SaveFile(string filePath, byte[] fileBytes);
        Task<string> SaveFile(string filePath, IFormFile file);
        Task<byte[]> GetFileContent(string filePath);
        Task<bool> DeleteFile(string filePath);
        Task<bool> DeleteFiles(IEnumerable<string> filePaths);

        
        Task<FileInspectedResult> InspectContent(Stream fileStream);
        Task<FileInspectedResult> InspectContent(byte[] fileBytes);
        Task<FileInspectedResult> InspectContent(IFormFile file);
        Task<FileInspectedResult> InspectContent(string filePath);

    }
}
