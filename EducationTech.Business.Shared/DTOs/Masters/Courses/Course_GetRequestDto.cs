using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Courses
{
    public class Course_GetRequestDto
    {
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public string OrderBy { get; set; } = "";
        public bool IsIncludeArchived { get; set; } = false;
        public bool IsIncludeNotPublished { get; set; } = false;
        public bool IsIncludeOwner { get; set; } = false;
        public bool IsIncludeRate { get; set; } = false;
        public bool BelongToCurrentUser { get; set; } = false;

        public bool CreatedByCurrentUser { get; set; } = false;
        
    }
}
