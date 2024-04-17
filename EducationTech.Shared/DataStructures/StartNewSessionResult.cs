using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Shared.DataStructures
{
    public struct StartNewSessionResult
    {
        public Guid SessionId { get; set; }
        public int MaxChunkSize { get; set; }
        public int TotalChunks { get; set; }
    }
}
