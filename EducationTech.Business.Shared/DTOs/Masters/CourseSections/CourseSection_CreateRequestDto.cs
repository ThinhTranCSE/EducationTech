using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.CourseSections
{
    public class CourseSection_CreateRequestDto : AbstractDto<CourseSection, CourseSection_CreateRequestDto>
    {
        public string Title { get; set; }
        public int CourseId { get; set; }
        public int Order { get; set; }
    }
}
