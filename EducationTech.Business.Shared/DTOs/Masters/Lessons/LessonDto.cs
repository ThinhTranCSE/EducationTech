using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
using EducationTech.Business.Shared.DTOs.Masters.Videos;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Lessons
{
    public class LessonDto : AbstractDto<Lesson, LessonDto>
    {
        public int Id { get; set; }
        public int CourseSectionId { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public LessonType Type { get; set; }
        public VideoDto Video { get; set; }
        public QuizDto Quiz { get; set; }
    }
}
