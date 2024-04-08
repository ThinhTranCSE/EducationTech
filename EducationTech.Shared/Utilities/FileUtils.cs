using EducationTech.Shared.Utilities.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EducationTech.Shared.Utilities
{
    public class FileUtils : IFileUtils
    {


        public async Task<string> SaveFileAsync(string filePath, Stream fileStream)
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

        public async Task<string> SaveFileAsync(string filePath, byte[] fileBytes)
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

        public async Task<string> SaveFileAsync(string filePath, IFormFile file)
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

        public async Task<bool> DeleteFileAsync(string filePath)
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

        public async Task<byte[]> GetFileContentAsync(string filePath)
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
    }
}
