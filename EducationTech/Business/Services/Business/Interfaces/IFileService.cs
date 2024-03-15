using EducationTech.Business.Services.Abstract;

namespace EducationTech.Business.Services.Business.Interfaces
{
    public interface IFileService : IService
    {
        Task<string> UploadFileAsync(IFormFile file);
        //Task<string> DownloadFileAsync();
    }
}
