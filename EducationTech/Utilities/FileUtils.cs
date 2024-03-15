using EducationTech.Utilities.Interfaces;

namespace EducationTech.Utilities
{
    public class FileUtils : IFileUtils
    {
        public async Task<string> SaveFileAsync(string filePath, Stream fileStream)
        {
            try
            {
                if(!Directory.Exists(Path.GetDirectoryName(filePath)))
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
    }
}
