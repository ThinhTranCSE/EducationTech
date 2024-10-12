using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Business.File;
using Microsoft.AspNetCore.Http;

namespace EducationTech.Business.Business.Interfaces
{
    public interface IFileService : IService
    {
        Task<IEnumerable<UploadedFileDto>> GetFileInformation(File_GetFileInformationRequestDto requestDto);
        Task<UploadedFileDto> UploadFile(IFormFile file);
        Task<File_PrepareResponseDto> StartLargeFileUploadSession(string fileName, long fileSize);
        Task<File_ChunkInfomationDto> UploadChunk(Guid sessionId, int index, IFormFile chunkFormFile);
        Task<File_GetFileContentDto> GetFile(Guid fileId);
        Task<File_GetFileContentDto> GetPlaylist(Guid streamId);
        Task<File_GetFileContentDto> GetSegment(Guid streamId, string segmentName);
    }
}
