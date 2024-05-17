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
    public class Quiz_CreateRequestDto : AbstractDto<Quiz, Quiz_CreateRequestDto>
    {
        public int LessonId { get; set; }
        public int TimeLimit { get; set; }
        public IEnumerable<Question_CreateQuizDto> Questions { get; set; } = new List<Question_CreateQuizDto>();

    }
}
