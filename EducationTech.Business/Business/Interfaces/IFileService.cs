using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Business.File;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.AspNetCore.Http;

namespace EducationTech.Business.Business.Interfaces
{
    public interface IFileService : IService
    {
        Task<UploadedFile> UploadFile(IFormFile file, Guid userId);
        Task<File_PrepareResponseDto> StartLargeFileUploadSession(string fileName, long fileSize, Guid userId);
        Task<File_ChunkInfomationDto> UploadChunk(Guid sessionId, int index, IFormFile chunkFormFile);
        Task<File_GetFileContentDto> GetFile(Guid fileId);
        Task<File_GetFileContentDto> GetPlaylist(string streamId);
        Task<File_GetFileContentDto> GetSegment(string streamId, string segmentName);
    }
}
