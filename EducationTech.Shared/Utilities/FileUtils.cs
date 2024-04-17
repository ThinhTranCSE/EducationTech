using EducationTech.Shared.DataStructures;
using EducationTech.Shared.Utilities.Interfaces;
using Microsoft.AspNetCore.Http;
using MimeDetective;
using MimeDetective.Engine;
using System.Collections.Immutable;
namespace EducationTech.Shared.Utilities
{
    public class FileUtils : IFileUtils
    {
        private readonly ContentInspector _inspector;

        public FileUtils(ContentInspector inspector)
        {
            _inspector = inspector;
        }
        public async Task<string> SaveFile(string filePath, Stream fileStream)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                using (var file = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(file);
                }
                return filePath;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> SaveFile(string filePath, byte[] fileBytes)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                await File.WriteAllBytesAsync(filePath, fileBytes);
                return filePath;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> SaveFile(string filePath, IFormFile file)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return filePath;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteFiles(IEnumerable<string> filePaths)
        {
            try
            {
                Parallel.ForEach(filePaths, filePath =>
                {
                    File.Delete(filePath);
                });
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<byte[]> GetFileContent(string filePath)
        {
            try
            {
                return await File.ReadAllBytesAsync(filePath);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<FileInspectedResult> InspectContent(byte[] fileBytes)
        {
            byte[] fileContents = fileBytes;
            var results = _inspector.Inspect(fileContents);
            string? extension = results.ByFileExtension().FirstOrDefault()?.Extension;
            string? mime = results.ByMimeType().FirstOrDefault()?.MimeType;
            return new FileInspectedResult
            {
                IsSuccess = extension != null && mime != null,
                Extension = extension,
                MimeType = mime
            };
        }
        public Task<FileInspectedResult> InspectContent(Stream fileStream)
        {
            byte[] fileContents = new byte[fileStream.Length];
            fileStream.Read(fileContents, 0, (int)fileStream.Length);
            return InspectContent(fileContents);
        }
        public Task<FileInspectedResult> InspectContent(IFormFile file)
        {
            byte[] fileContents = new byte[file.Length];
            using var stream = file.OpenReadStream();
            stream.Read(fileContents, 0, (int)file.Length);
            return InspectContent(fileContents);
        }
        public async Task<FileInspectedResult> InspectContent(string filePath)
        {
            byte[] fileContents = File.ReadAllBytes(filePath);
            return await InspectContent(fileContents);
        }
    }
}
