using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Shared.DataStructures
{
    public class FileInfomation
    {
        public ISet<int> AlreadyPersistedChunks = new HashSet<int>();
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
            AlreadyPersistedChunks.Add(index);
        }
    }
}
