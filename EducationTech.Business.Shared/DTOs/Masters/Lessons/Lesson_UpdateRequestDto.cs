using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Lessons
{
    public class Lesson_UpdateRequestDto : AbstractDto<Lesson, Lesson_UpdateRequestDto>
    {
        public string? Title { get; set; }
        public int? Order { get; set; }
        public int? VideoId { get; set; }
    }
}
