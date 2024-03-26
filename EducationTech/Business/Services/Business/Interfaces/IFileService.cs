using EducationTech.Business.DTOs.Business.File;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Services.Abstract;

namespace EducationTech.Business.Services.Business.Interfaces
{
    public interface IFileService : IService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<File_PrepareResponseDto> PrepareUploadLargeFileAsync(string fileName, long fileSize, User userUpload);
        Task<File_ChunkInfomationDto> UploadChunk(string chunkName, long chunkSize, IFormFile chunkFormFile);
        Task<File_MergeResponseDto> MergeFile(Guid fileId);
        Task<File_GetFileContentDto> GetFile(Guid fileId);

        Task<File_GetFileContentDto> GetPlaylist(string streamId);
        Task<File_GetFileContentDto> GetSegment(string streamId, string segmentName);
    }
}
