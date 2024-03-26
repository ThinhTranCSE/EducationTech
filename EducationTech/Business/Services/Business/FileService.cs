using EducationTech.Business.DTOs.Business.File;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Business.Services.Business.Interfaces;
using EducationTech.Exceptions.Http;
using EducationTech.Utilities.Interfaces;
using HeyRed.Mime;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Web;
namespace EducationTech.Business.Services.Business
{
    public class FileService : IFileService
    {
        private const int CHUNK_SIZE = 1024 * 1024;
        private readonly IFileUtils _fileUtils;
        private readonly GlobalUsings _globalUsings;
        private readonly IUploadedFileRepository _uploaedFileRepository;
        private readonly ICacheService _cacheService;

        public FileService(IFileUtils fileUtils, GlobalUsings globalUsings, IUploadedFileRepository uploadedFileRepository, ICacheService cacheService)
        {
            _fileUtils = fileUtils;
            _globalUsings = globalUsings;
            _uploaedFileRepository = uploadedFileRepository;
            _cacheService = cacheService;

        }

        public async Task<File_GetFileContentDto> GetFile(Guid fileId)
        {
            var fileEntity = await _uploaedFileRepository.GetSingle(f => f.Id == fileId);
            if (fileEntity == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "File not found");
            }
            byte[] content = await _fileUtils.GetFileContentAsync(Path.Combine(_globalUsings.StaticFilesPath, fileEntity.Path));
            return new File_GetFileContentDto
            {
                Content = content,
                ContentType = MimeTypesMap.GetMimeType(fileEntity.OriginalFileName)
            };
        }

        public async Task<File_GetFileContentDto> GetPlaylist(string streamId)
        {
            string streamDirectory = Path.Combine(_globalUsings.StaticFilesPath, "Streams", streamId);

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
            byte[] bytes = await _fileUtils.GetFileContentAsync(playlistFilePath);

            return new File_GetFileContentDto
            {
                Content = bytes,
                ContentType = "application/x-mpegURL"
            };
        }

        public async Task<File_GetFileContentDto> GetSegment(string streamId, string segmentName)
        {
            string streamDirectory = Path.Combine(_globalUsings.StaticFilesPath, "Streams", streamId);

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
            byte[] bytes = await _fileUtils.GetFileContentAsync(playlistFilePath);

            return new File_GetFileContentDto
            {
                Content = bytes,
                ContentType = "video/MP2T"
            };

        }

        public async Task<File_PrepareResponseDto> PrepareUploadLargeFileAsync(string fileName, long fileSize, User userUpload)
        {
            var file = await _uploaedFileRepository.Insert(new UploadedFile
            {
                OriginalFileName = fileName,
                Size = fileSize,
                IsCompleted = false,
                IsPublic = false,
                UserId = userUpload.Id,
            });

            var chunks = new List<File_ChunkInfomationDto>();
            long remainingSize = fileSize;
            long currentByte = 0;
            for (int i = 0; remainingSize > 0; i++)
            {
                long chunkSize = Math.Min(remainingSize, CHUNK_SIZE);
                chunks.Add(new File_ChunkInfomationDto
                {
                    ChunkName = $"{file!.Id}.part{i}",
                    Start = currentByte,
                    End = currentByte + chunkSize - 1,
                    ChunkSize = chunkSize,
                    Index = i
                });
                currentByte += chunkSize;
                remainingSize -= chunkSize;
            }
            return new File_PrepareResponseDto
            {
                File = file,
                Chunks = chunks
            };
        }

        public async Task<File_ChunkInfomationDto> UploadChunk(string chunkName, long chunkSize, IFormFile chunkFormFile)
        {
            if (chunkFormFile.Length != chunkSize)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "ChunkFormfile size is different from chunkSize");
            }
            string path = Path.Combine(_globalUsings.StaticFilesPath, "Temps", chunkName);
            await _fileUtils.SaveFileAsync(path, chunkFormFile);
            return new File_ChunkInfomationDto
            {
                ChunkName = chunkName,
                ChunkSize = chunkFormFile.Length,
                Index = chunkName.Split(".part")[1].ConvertTo<int>()
            };
        }

        public async Task<File_MergeResponseDto> MergeFile(Guid fileId)
        {
            var fileEntity = await _uploaedFileRepository.GetSingle(f => f.Id == fileId);
            if (fileEntity == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "File not found");
            }
            string tempDirectory = Path.Combine(_globalUsings.StaticFilesPath, "Temps");
            string[] chunkFiles = Directory.GetFiles(tempDirectory, $"{fileEntity.Id}.part*");
            chunkFiles = chunkFiles.OrderBy(f =>
            {
                // Sort by index
                return f.Split(".part")[1].ConvertTo<int>();
            }).ToArray();
            string orginalExtension = Path.GetExtension(fileEntity.OriginalFileName);

            if(!Directory.Exists(Path.Combine(_globalUsings.StaticFilesPath, "Uploads")))
            {
                Directory.CreateDirectory(Path.Combine(_globalUsings.StaticFilesPath, "Uploads"));
            }

            using(var fileStream = new FileStream(Path.Combine(_globalUsings.StaticFilesPath, "Uploads", $"{fileEntity.Id}{orginalExtension}"), FileMode.Create))
            {
                foreach (string chunkFile in chunkFiles)
                {
                    using(var chunkFileStream = new FileStream(chunkFile, FileMode.Open))
                    {
                        await chunkFileStream.CopyToAsync(fileStream);
                    }
                }
                if(fileStream.Length != fileEntity.Size)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "File size is different from the original size");
                }
            }
            fileEntity.IsCompleted = true;
            fileEntity.Path = Path.Combine("Uploads", $"{fileEntity.Id}{orginalExtension}");
            await _uploaedFileRepository.Update(fileEntity);

            foreach (var chunkFile in chunkFiles)
            {
                File.Delete(chunkFile);
            }
            
            return new File_MergeResponseDto
            {
                File = fileEntity
            };

        } 

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            string path = Path.Combine(_globalUsings.StaticFilesPath, "Uploads", file.FileName);
            return await _fileUtils.SaveFileAsync(path, file);
        }
    }
}
