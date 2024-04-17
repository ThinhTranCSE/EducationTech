using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Shared.DataStructures
{
    public struct FileInspectedResult
    {
        public bool IsSuccess { get; set; }
        public string? Extension { get; set; }
        public string? MimeType { get; set; }
    }
}
