using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Shared.DataStructures
{
    public class FileInfomation
    {
        private object _lock = new object();
        private ISet<int> AlreadyPersistedChunks = new HashSet<int>();
        public string OriginalFileName { get; set; }
        public long Size { get; set; }
        public int ChunkSize { get; set; }

        public FileInfomation(string originalFileName, long size, int chunkSize)
        {
            OriginalFileName = originalFileName;
            Size = size;
            ChunkSize = chunkSize;
        }
        public int TotalChunks => (int)Math.Ceiling((double)Size / ChunkSize);

        public void MarkChunkAsPersisted(int index)
        {
            lock (_lock)
            {
                AlreadyPersistedChunks.Add(index);
            }
        }

        public int PersistedChunksCount()
        {
            int count;
            lock (_lock)
            {
                count = AlreadyPersistedChunks.Count;
            }
            return count;
        }

        public bool IsCompleted()
        {
            bool isCompleted;
            lock (_lock)
            {
                isCompleted = AlreadyPersistedChunks.Count == TotalChunks;
            }
            return isCompleted;
        }
    }
}
