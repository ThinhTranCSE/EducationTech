using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.File;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Shared.Utilities.Interfaces;
using HeyRed.Mime;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Web;
using EducationTech.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using EducationTech.Storage;
using EducationTech.Shared.DataStructures;
using MySqlX.XDevAPI;
using System.IO;
using AutoMapper;

namespace EducationTech.Business.Business
{
    public class FileService : IFileService
    {
        private readonly IFileUtils _fileUtils;
        private readonly GlobalUsings _globalUsings;
        private readonly IUploadedFileRepository _uploadedFileRepository;
        private readonly UploadFileSessionManager _uploadFileSessionManager;
        private readonly IMapper _mapper;

        public FileService(
            IFileUtils fileUtils, 
            GlobalUsings globalUsings, 
            IUploadedFileRepository uploadedFileRepository, 
            UploadFileSessionManager uploadFileSessionManager,
            IMapper mapper
            )
        {
            _fileUtils = fileUtils;
            _globalUsings = globalUsings;
            _uploadedFileRepository = uploadedFileRepository;
            _uploadFileSessionManager = uploadFileSessionManager;
            _mapper = mapper;
        }

        public async Task<File_GetFileContentDto> GetFile(Guid fileId)
        {
            var fileEntity = await _uploadedFileRepository.GetSingle(f => f.Id == fileId);
            if (fileEntity == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "File not found");
            }
            string filePath = Path.Combine(_globalUsings.StaticFilesPath, fileEntity.Path);
            byte[] content = await _fileUtils.GetFileContent(filePath);
            string extension = fileEntity.Extension;
            return new File_GetFileContentDto
            {
                Content = content,
                ContentType = MimeTypesMap.GetMimeType(extension)
            };
        }

        public async Task<File_GetFileContentDto> GetPlaylist(string streamId)
        {
            string streamDirectory = Path.Combine(_globalUsings.StreamFilesPath, streamId);

            // Check if the directory exists
            if (!Directory.Exists(streamDirectory))
            {
                throw new HttpException(404);
            }
            string playlistFilePath = Path.Combine(streamDirectory, "input.m3u8");
            if (!File.Exists(playlistFilePath))
            {
                throw new HttpException(404);
            }
            byte[] bytes = await _fileUtils.GetFileContent(playlistFilePath);

            return new File_GetFileContentDto
            {
                Content = bytes,
                ContentType = "application/x-mpegURL"
            };
        }

        public async Task<File_GetFileContentDto> GetSegment(string streamId, string segmentName)
        {
            string streamDirectory = Path.Combine(_globalUsings.StreamFilesPath, streamId);

            // Check if the directory exists
            if (!Directory.Exists(streamDirectory))
            {
                throw new HttpException(404);
            }
            string playlistFilePath = Path.Combine(streamDirectory, $"{segmentName}.ts");
            if (!File.Exists(playlistFilePath))
            {
                throw new HttpException(404);
            }
            byte[] bytes = await _fileUtils.GetFileContent(playlistFilePath);

            return new File_GetFileContentDto
            {
                Content = bytes,
                ContentType = "video/MP2T"
            };

        }
        public async Task<File_PrepareResponseDto> StartLargeFileUploadSession(string fileName, long fileSize, Guid userId)
        {
            var startNewSessionResult = _uploadFileSessionManager.StartNewSession(userId, fileName, fileSize);
            return new File_PrepareResponseDto
            {
                SessionId = startNewSessionResult.SessionId,
                ChunkSize = startNewSessionResult.MaxChunkSize,
                TotalChunks = startNewSessionResult.TotalChunks,
                OriginalFileName = fileName
            };
        }

        public async Task<File_ChunkInfomationDto> UploadChunk(Guid sessionId, int index, IFormFile chunkFormFile)
        {
            if(chunkFormFile.Length == 0)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "ChunkFormFile size is 0");
            }
            if(chunkFormFile.Length > _uploadFileSessionManager.MaxChunkSize)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "ChunkFormFile size is greater than maximum chunk size");
            }
            
            if (!_uploadFileSessionManager.IsSessionAvailable(sessionId))
            {
                throw new HttpException(HttpStatusCode.NotFound, "Session is not available");
            }
            
            //prevent session dispose from cleaner thread
            _uploadFileSessionManager.StartProcessing(sessionId);
            
            //save chunk file
            int totalChunks = _uploadFileSessionManager.GetTotalChunks(sessionId);
            string chunkName = $"{sessionId}.total{totalChunks}.part{index}";
            string path = Path.Combine(_globalUsings.TempFilesPath, chunkName);
            await _fileUtils.SaveFile(path, chunkFormFile);

            //mark chunk as persisted
            _uploadFileSessionManager.MarkChunkAsPersisted(sessionId, index);

            //if all chunks are persisted, merge them to a single file
            bool isSessionCompleted = _uploadFileSessionManager.IsSessionCompleted(sessionId);
            if(isSessionCompleted)
            {
                await MergeFile(sessionId);
            }

            //cleaner thread can dispose session
            _uploadFileSessionManager.StopProcessing(sessionId);

            return new File_ChunkInfomationDto
            {
                ChunkSize = chunkFormFile.Length,
                Index = index,
                Progress = _uploadFileSessionManager.GetSessionProgress(sessionId),
                IsSessionCompleted = isSessionCompleted
            };
        }

        private async Task<bool> MergeFile(Guid sessionId)
        {
            string tempDirectory = _globalUsings.TempFilesPath;
            string[] chunkFiles = Directory.GetFiles(tempDirectory, $"{sessionId}.total*.part*", SearchOption.TopDirectoryOnly);
            try
            {
                chunkFiles = chunkFiles.OrderBy(f =>
                {
                    // Sort by index
                    return f.Split(".part")[1].ConvertTo<int>();
                }).ToArray();
                if (chunkFiles.Length == 0)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "No chunk files found");
                }

                var fileInfo = _uploadFileSessionManager.GetSessionFileInfomation(sessionId);
                string originalFileExtension = Path.GetExtension(fileInfo.OriginalFileName);
                var inspectResult = await _fileUtils.InspectContent(chunkFiles[0]);
                // Check if the extension of the original file matches the content
                string? extension = inspectResult.Extension;
                if (extension == null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "File extension can not detect from file content");
                }
                if ($".{extension}" != originalFileExtension)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "Original file extesion doesn't macth with it content");
                }

                UploadedFile fileEntity = new UploadedFile
                {
                    OriginalFileName = fileInfo.OriginalFileName,
                    Size = fileInfo.Size,
                    Extension = inspectResult.Extension,
                    IsCompleted = true,
                    UserId = _uploadFileSessionManager.GetSessionOwner(sessionId)
                };

                await _uploadedFileRepository.Insert(fileEntity, true);

                string categoryDirectory = _globalUsings.PathCollection[extension];
                string fileName = $"{fileEntity.Id}.{extension}";

                using (var fileStream = new FileStream(Path.Combine(categoryDirectory, fileName), FileMode.Create))
                {
                    foreach (string chunkFile in chunkFiles)
                    {
                        using (var chunkFileStream = new FileStream(chunkFile, FileMode.Open))
                        {
                            await chunkFileStream.CopyToAsync(fileStream);
                        }
                    }
                    if (fileStream.Length != fileEntity.Size)
                    {
                        throw new HttpException(HttpStatusCode.BadRequest, "File size is different from the original size");
                    }
                }
                fileEntity.Path = Path.Combine(Path.GetFileName(categoryDirectory), fileName);
                await _uploadedFileRepository.Update(fileEntity, true);

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _fileUtils.DeleteFiles(chunkFiles);
            }
        }

        public async Task<UploadedFile> UploadFile(IFormFile file, Guid userId)
        {
            if(file.Length == 0)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "File size is 0");
            }
            if(file.Length > _globalUsings.UploadChunkSize)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "File is large, please use method for large file");
            }
            var inspectResult = await _fileUtils.InspectContent(file);

            string originalFileExtension = Path.GetExtension(file.FileName);
            // Check if the extension of the original file matches the content
            string? extension = inspectResult.Extension;
            if (extension == null)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "File extension can not detect from file content");
            }
            if ($".{extension}" != originalFileExtension)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Original file extesion doesn't macth with it content");
            }
            UploadedFile fileEntity = new UploadedFile
            {
                OriginalFileName = file.FileName,
                Size = file.Length,
                Extension = inspectResult.Extension,
                IsCompleted = true,
                UserId = userId
            };
            await _uploadedFileRepository.Insert(fileEntity, true);

            string categoryDirectory = _globalUsings.PathCollection[extension];
            string fileName = $"{fileEntity.Id}.{extension}";

            // Save the file
            using (var fileStream = new FileStream(Path.Combine(categoryDirectory, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            fileEntity.Path = Path.Combine(Path.GetFileName(categoryDirectory), fileName);

            await _uploadedFileRepository.Update(fileEntity, true);

            return fileEntity;
        }
    }
}
