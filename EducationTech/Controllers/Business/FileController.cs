using AutoMapper;
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
        private readonly IMapper _mapper;
        public FileController(EducationTechContext context, IAuthService authService, IFileService fileService, IMapper mapper) : base(context, authService)
        {
            _fileService = fileService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UploadedFileDto>> GetFileInformation([FromQuery] File_GetFileInformationRequestDto requestDto)
        {
            var result =  await _fileService.GetFileInformation(requestDto, CurrentUser);
            return result;
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
        public async Task<UploadedFileDto> Upload([FromForm] IFormFile file)
        {
            var result =  await _fileService.UploadFile(file, CurrentUser!.Id);
            return result;
        }


        [HttpGet("Stream/{streamId}/input.m3u8")]
        [SkipRestructurePhase]
        public async Task<IActionResult> GetPlaylist(Guid streamId)
        {
            //return m3u8 file at result
            var fileContent = await _fileService.GetPlaylist(streamId);
            return File(fileContent.Content, fileContent.ContentType);
        }

        [HttpGet("Stream/{streamId}/{segmentName}.ts")]
        [SkipRestructurePhase]
        public async Task<IActionResult> GetSegment(Guid streamId, string segmentName)
        {
            //return ts file at result
            var fileContent = await _fileService.GetSegment(streamId, segmentName);
            return File(fileContent.Content, fileContent.ContentType);
        }

        [HttpGet("{fileId}")]
        [AllowAnonymous]
        [SkipRestructurePhase]
        public async Task<IActionResult> GetFile(Guid fileId)
        {
            var file = await _fileService.GetFile(fileId);
            return File(file.Content, file.ContentType);
        }
    }
}
