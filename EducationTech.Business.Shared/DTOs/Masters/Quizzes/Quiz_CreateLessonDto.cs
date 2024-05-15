using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Questions;
using EducationTech.DataAccess.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Quizzes
{
    public class Quiz_CreateLessonDto : AbstractDto<Quiz, Quiz_CreateLessonDto>
    {
        public int TimeLimit { get; set; }
        public ICollection<Question_CreateLessonDto> Questions { get; set; } = new List<Question_CreateLessonDto>();
    }
}
