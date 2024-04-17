using EducationTech.Annotations;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.File;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace EducationTech.Controllers.Business
{
    [Authorize]
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;
        public FileController(EducationTechContext context, IAuthService authService, IFileService fileService) : base(context, authService)
        {
            _fileService = fileService;
        }

        [HttpPost("Session")]
        public async Task<File_PrepareResponseDto> Prepare(File_PrepareRequestBodyDto requestBody)
        {
            return await _fileService.StartLargeFileUploadSession(requestBody.FileName, requestBody.FileSize, CurrentUser!.Id);
        }

        [HttpPost("Session/Chunk")]
        public async Task<File_ChunkInfomationDto> UploadChunk([FromForm] File_UploadChunkRequestBodyDto requestBody)
        {
            return await _fileService.UploadChunk(requestBody.SessionId, requestBody.Index, requestBody.ChunkFormFile);
        }

        [HttpPost("Upload")]
        public async Task<UploadedFile> Upload([FromForm] IFormFile file)
        {
            return await _fileService.UploadFile(file, CurrentUser!.Id);
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
