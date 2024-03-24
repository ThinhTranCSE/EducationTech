using EducationTech.Annotations;
using EducationTech.Business.Controllers.Abstract;
using EducationTech.Business.DTOs.Business.File;
using EducationTech.Business.Services.Business.Interfaces;
using EducationTech.Databases;
using EducationTech.Utilities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace EducationTech.Business.Controllers.Business
{
    [Authorize]
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;
        public FileController(EducationTechContext context, IAuthUtils authUtils , IFileService fileService) : base(context, authUtils)
        {
            _fileService = fileService;
        }

        [HttpPost("Prepare")]
        public async Task<File_PrepareResponseDto> Prepare(File_PrepareRequestBodyDto requestBody)
        {
            return await _fileService.PrepareUploadLargeFileAsync(requestBody.Name, requestBody.Size, CurrentUser);
        }

        [HttpPost("Upload/Chunk")]
        public async Task<File_ChunkInfomationDto> UploadChunk([FromForm] File_UploadChunkRequestBodyDto requestBody)
        {
            return  await _fileService.UploadChunk(requestBody.ChunkName, requestBody.ChunkSize, requestBody.ChunkFormFile);
        }

        [HttpPost("Upload/Merge/{fileId}")]
        public async Task<File_MergeResponseDto> Merge(Guid fileId)
        {
            return await _fileService.MergeFile(fileId);
        }

        [HttpGet("Streams/{streamId}/input.m3u8")]
        [SkipRestructurePhase]
        public async Task<IActionResult> GetPlaylist(string streamId)
        {
            //return m3u8 file at result
            var fileContent = await _fileService.GetPlaylist(streamId);
            return File(fileContent.Content, fileContent.ContentType);
        }

        [HttpGet("Streams/{streamId}/{segmentName}.ts")]
        [SkipRestructurePhase]
        public async Task<IActionResult> GetSegment(string streamId, string segmentName)
        {
            //return ts file at result
            var fileContent = await _fileService.GetSegment(streamId, segmentName);
            return File(fileContent.Content, fileContent.ContentType);
        }

        [HttpGet("{fileId}")]
        [SkipRestructurePhase]
        public async Task<IActionResult> GetFile(Guid fileId)
        {
            var file = await _fileService.GetFile(fileId);
            return File(file.Content, file.ContentType);
        }
    }
}
