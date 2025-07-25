﻿using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.File;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.MessageQueue.VideoConvert;
using EducationTech.Shared.DataStructures;
using EducationTech.Shared.Extensions;
using EducationTech.Shared.Utilities.Interfaces;
using EducationTech.Storage;
using EducationTech.Storage.Enums;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EducationTech.Business.Business;

public class FileService : IFileService
{
    private readonly IFileUtils _fileUtils;
    private readonly GlobalReference _globalUsings;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionService _sessionService;
    private readonly UploadFileSessionManager _uploadFileSessionManager;
    private readonly IMapper _mapper;
    private readonly VideoConvertPublisher _videoConvertPublisher;
    public FileService(
        IFileUtils fileUtils,
        IUnitOfWork unitOfWork,
        ISessionService sessionService,
        UploadFileSessionManager uploadFileSessionManager,
        IMapper mapper,
        VideoConvertPublisher videoConvertPublisher
        )
    {
        _fileUtils = fileUtils;
        _globalUsings = GlobalReference.Instance;
        _unitOfWork = unitOfWork;
        _sessionService = sessionService;
        _uploadFileSessionManager = uploadFileSessionManager;
        _mapper = mapper;
        _videoConvertPublisher = videoConvertPublisher;
    }

    public async Task<File_GetFileContentDto> GetFile(Guid fileId)
    {
        var fileEntity = _unitOfWork.UploadedFiles.Find(f => f.Id == fileId).FirstOrDefault();
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

    public async Task<File_GetFileContentDto> GetPlaylist(Guid streamId)
    {
        string streamDirectory = Path.Combine(_globalUsings.StreamFilesPath, streamId.ToString());

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

    public async Task<File_GetFileContentDto> GetSegment(Guid streamId, string segmentName)
    {
        string streamDirectory = Path.Combine(_globalUsings.StreamFilesPath, streamId.ToString());

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
    public async Task<File_PrepareResponseDto> StartLargeFileUploadSession(string fileName, long fileSize)
    {
        var userId = _sessionService.CurrentUser!.Id;
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
        if (chunkFormFile.Length == 0)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "ChunkFormFile size is 0");
        }
        if (chunkFormFile.Length > _uploadFileSessionManager.MaxChunkSize)
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
        if (isSessionCompleted)
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

            _unitOfWork.UploadedFiles.Add(fileEntity);
            _unitOfWork.SaveChanges();

            string categoryDirectory = _globalUsings.PathCollection[extension];
            string fileName = $"{fileEntity.Id}.{extension}";
            string filePath = Path.Combine(categoryDirectory, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
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
            FileType? fileType = _globalUsings.FileTypeCollection[extension];
            fileEntity.FileType = fileType ?? FileType.Unknown;

            _unitOfWork.SaveChanges();

            if (fileType != null)
            {
                switch (fileType)
                {
                    case FileType.Image:
                        _unitOfWork.Images.Add(new Image
                        {
                            FileId = fileEntity.Id,
                            Url = $"api/v1/File/{fileEntity.Id}"
                        });
                        break;
                    case FileType.Video:
                        _unitOfWork.Videos.Add(new Video
                        {
                            FileId = fileEntity.Id,
                            Url = $"api/v1/File/Stream/{fileEntity.Id}/input.m3u8"
                        });
                        _videoConvertPublisher.Publish(new VideoConvertMessage
                        {
                            OriginalVideoPath = filePath,
                            ConvertedVideoDirectory = Path.Combine(categoryDirectory, fileEntity.Id.ToString())
                        });
                        break;
                }
            }
            _unitOfWork.SaveChanges();
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

    public async Task<UploadedFileDto> UploadFile(IFormFile file)
    {
        var userId = _sessionService.CurrentUser!.Id;
        if (file.Length == 0)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "File size is 0");
        }
        if (file.Length > _globalUsings.UploadChunkSize)
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
        _unitOfWork.UploadedFiles.Add(fileEntity);
        _unitOfWork.SaveChanges();

        Console.WriteLine(_globalUsings.PathCollection[extension]);
        string categoryDirectory = _globalUsings.PathCollection[extension];
        string fileName = $"{fileEntity.Id}.{extension}";
        string filePath = Path.Combine(categoryDirectory, fileName);
        // Save the file
        await _fileUtils.SaveFile(filePath, file);

        fileEntity.Path = filePath;

        FileType? fileType = _globalUsings.FileTypeCollection[extension];

        fileEntity.FileType = fileType ?? FileType.Unknown;

        _unitOfWork.SaveChanges();

        if (fileType != null)
        {
            switch (fileType)
            {
                case FileType.Image:
                    _unitOfWork.Images.Add(new Image
                    {
                        FileId = fileEntity.Id,
                        Url = $"api/v1/File/{fileEntity.Id}"
                    });
                    break;
                case FileType.Video:
                    _unitOfWork.Videos.Add(new Video
                    {
                        FileId = fileEntity.Id,
                        Url = $"api/v1/File/Stream/{fileEntity.Id}/input.m3u8"
                    });
                    _videoConvertPublisher.Publish(new VideoConvertMessage
                    {
                        OriginalVideoPath = filePath,
                        ConvertedVideoDirectory = Path.Combine(categoryDirectory, fileEntity.Id.ToString())
                    });
                    break;
            }
        }
        _unitOfWork.SaveChanges();
        return _mapper.Map<UploadedFileDto>(fileEntity);
    }

    public async Task<IEnumerable<UploadedFileDto>> GetFileInformation(File_GetFileInformationRequestDto requestDto)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, "User is not authenticated");
        }

        var fileQuery = _unitOfWork.UploadedFiles.GetAll();
        fileQuery = fileQuery
            .Include(f => f.Image)
            .Include(f => f.Video)
            .Where(f => f.UserId == currentUser.Id);

        var files = await fileQuery.ToListAsync();

        return _mapper.ProjectTo<UploadedFileDto>(files.AsQueryable());
    }

}
