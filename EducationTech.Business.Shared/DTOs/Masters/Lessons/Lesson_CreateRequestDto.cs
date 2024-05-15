using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Lessons
{
    public class Lesson_CreateRequestDto : AbstractDto<Lesson, Lesson_CreateRequestDto>
    {
        public string Title { get; set; }
        public int CourseSectionId { get; set; }
        public int Order { get; set; }
        public LessonType Type { get; set; }
        public int? VideoId { get; set; }
        public Quiz_CreateLessonDto? Quiz { get; set; }
    }
}
