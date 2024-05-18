using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Business.File;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.AspNetCore.Http;

namespace EducationTech.Business.Business.Interfaces
{
    public interface IFileService : IService
    {
        Task<IEnumerable<UploadedFileDto>> GetFileInformation(File_GetFileInformationRequestDto requestDto, User? currentUser);
        Task<UploadedFileDto> UploadFile(IFormFile file, Guid userId);
        Task<File_PrepareResponseDto> StartLargeFileUploadSession(string fileName, long fileSize, Guid userId);
        Task<File_ChunkInfomationDto> UploadChunk(Guid sessionId, int index, IFormFile chunkFormFile);
        Task<File_GetFileContentDto> GetFile(Guid fileId);
        Task<File_GetFileContentDto> GetPlaylist(Guid streamId);
        Task<File_GetFileContentDto> GetSegment(Guid streamId, string segmentName);
    }
}
