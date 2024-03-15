using EducationTech.Business.Services.Business.Interfaces;
using EducationTech.Utilities.Interfaces;

namespace EducationTech.Business.Services.Business
{
    public class FileService : IFileService
    {
        private readonly IFileUtils _fileUtils;
        private readonly GlobalUsings _globalUsings;

        public FileService(IFileUtils fileUtils, GlobalUsings globalUsings)
        {
            _fileUtils = fileUtils;
            _globalUsings = globalUsings;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            string path = Path.Combine(_globalUsings.StaticFilesPath, "Uploads", file.FileName);
            return await _fileUtils.SaveFileAsync(path, file);
        }
    }
}
