using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Courses
{
    public class Course_GetByIdRequestDto
    {
        public bool BelongToCurrentUser { get; set; } = false;
        public bool IsGetGetail { get; set; } = false;
    }
}
