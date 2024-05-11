using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Lessons;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.CourseSections
{
    public class CourseSectionDto : AbstractDto<CourseSection, CourseSectionDto>
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Title { get; set; }
        public int Order { get; set; }

        public ICollection<LessonDto> Lessons { get; set; } = new List<LessonDto>();
    }
}
