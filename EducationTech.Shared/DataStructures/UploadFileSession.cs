using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Shared.DataStructures
{
    public class UploadFileSession
    {
        private const long DEFAULT_TIME_OUT = 60 * 60; //seconds

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModifiedAt { get; private set; }
        public long TimeOut { get; set; }
        public FileInfomation FileInfo { get; set; }
        public int SuccessfulChunks => FileInfo.AlreadyPersistedChunks.Count;
        public bool IsProcessing { get; set; } = false;
        public bool IsCompleted => SuccessfulChunks == FileInfo.TotalChunks;
        public bool IsExpired
        {
            get
            {
                TimeSpan span = DateTime.Now - LastModifiedAt;
                return span.TotalSeconds >= TimeOut;
            }
        }
        public double Progress
        {
            get
            {
                if (FileInfo.TotalChunks == 0)
                {
                    return 0;
                }
                return SuccessfulChunks / (FileInfo.TotalChunks * 1f);
            }
        }
        public UploadFileSession(Guid userId, FileInfomation fileInfo, long timeOut = DEFAULT_TIME_OUT)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CreatedAt = DateTime.Now;
            LastModifiedAt = DateTime.Now;
            TimeOut = timeOut;
            FileInfo = fileInfo;
        }

        public void MarkChunkAsPersisted(int index)
        {
            FileInfo.MarkChunkAsPersisted(index);
            LastModifiedAt = DateTime.Now;
        }
    }
}
