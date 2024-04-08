using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Business.File;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.AspNetCore.Http;

namespace EducationTech.Business.Business.Interfaces
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
