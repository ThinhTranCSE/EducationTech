using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Answers;
using EducationTech.DataAccess.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Questions
{
    public class Question_CreateLessonDto : AbstractDto<Question, Question_CreateLessonDto>
    {
        public string Content { get; set; }
        public ICollection<Answer_CreateLessonDto> Answers { get; set; } = new List<Answer_CreateLessonDto>();
    }
}
