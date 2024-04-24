using EducationTech.Storage;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Shared.DataStructures
{
    public class UploadFileSessionManager : IDisposable
    {
        //private const int DEFAULT_CHUNK_SIZE = 1024 * 1024; //1MB
        //private const long DEFAULT_TIME_OUT = 60 * 60; //seconds
        private Thread _cleanerThread;
        public int MaxChunkSize { get; private set; }
        public long SessionTimeOut { get; private set; }
        public GlobalUsings _globalUsings { get; set; }

        private Dictionary<Guid, UploadFileSession> _sessions = new Dictionary<Guid, UploadFileSession>();
        public UploadFileSessionManager(GlobalUsings globalUsings, IHostApplicationLifetime lifeTime)
        {
            MaxChunkSize = globalUsings.UploadChunkSize;
            SessionTimeOut = globalUsings.UploadSessionTimeOut;

            _globalUsings = new GlobalUsings();
            lifeTime.ApplicationStopping.Register(Dispose);
            _cleanerThread = new Thread(ExpiredSessionCleaner);
            _cleanerThread.Start();
        }

        public StartNewSessionResult StartNewSession(Guid userId, string originalFileName, long fileSize)
        {
            var fileInfo = new FileInfomation(originalFileName, fileSize, MaxChunkSize);
            var session = new UploadFileSession(userId, fileInfo, SessionTimeOut);
            _sessions.Add(session.Id, session);
            return new StartNewSessionResult
            {
                SessionId = session.Id,
                MaxChunkSize = MaxChunkSize,
                TotalChunks = fileInfo.TotalChunks
            };
        }

        public void StartProcessing(Guid sessionId)
        {
            if (_sessions.ContainsKey(sessionId))
            {
                _sessions[sessionId].IsProcessing = true;
            }
        }

        public void StopProcessing(Guid sessionId)
        {
            if (_sessions.ContainsKey(sessionId))
            {
                _sessions[sessionId].IsProcessing = false;
            }
        }

        public bool IsSessionAvailable(Guid sessionId)
        {
            if (_sessions.ContainsKey(sessionId))
            {
                return !_sessions[sessionId].IsCompleted;
            }
            return false;
        }

        public bool IsSessionCompleted(Guid sessionId)
        {
            if (_sessions.ContainsKey(sessionId))
            {
                return _sessions[sessionId].IsCompleted;
            }
            return false;
        }

        public double GetSessionProgress(Guid sessionId)
        {
            if (_sessions.ContainsKey(sessionId))
            {
                return _sessions[sessionId].Progress;
            }
            return 0;
        }


        public int GetTotalChunks(Guid sessionId)
        {
            if (_sessions.ContainsKey(sessionId))
            {
                return _sessions[sessionId].FileInfo.TotalChunks;
            }
            return 0;
        }

        public void MarkChunkAsPersisted(Guid sessionId, int index)
        {
            if (_sessions.ContainsKey(sessionId))
            {
                _sessions[sessionId].MarkChunkAsPersisted(index);
            }
        }

        public FileInfomation GetSessionFileInfomation(Guid sessionId)
        {
            if (_sessions.ContainsKey(sessionId))
            {
                return _sessions[sessionId].FileInfo;
            }
            return null;
        }

        public Guid GetSessionOwner(Guid sessionId)
        {
            if (_sessions.ContainsKey(sessionId))
            {
                return _sessions[sessionId].UserId;
            }
            return Guid.Empty;
        }
        private void ExpiredSessionCleaner()
        {
            TimeSpan sleepTime = TimeSpan.FromSeconds(SessionTimeOut);
            try
            {
                while (true)
                {
                    foreach (var session in _sessions.Values)
                    {
                        if (session.IsCompleted || session.IsExpired)
                        {
                            if (!session.IsProcessing)
                            {
                                RemoveSession(session.Id);
                            }
                        }
                    }
                    Thread.Sleep(sleepTime);
                }
            }
            catch (ThreadInterruptedException)
            {
                return;
            }
            catch (ThreadAbortException)
            {
                return;
            }
            
        }

        private void RemoveSession(Guid sessionId)
        {
            _sessions.Remove(sessionId);
            string tempDirectory = _globalUsings.TempFilesPath;
            string[] chunkFiles = Directory.GetFiles(tempDirectory, $"{sessionId}.total*.part*", SearchOption.TopDirectoryOnly);
            Parallel.ForEach(chunkFiles, (chunkPath) =>
            {
                File.Delete(chunkPath);
            });
        }
        public void Dispose()
        {
            _cleanerThread.Interrupt();
        }
    }
}
