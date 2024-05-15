using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Answers
{
    public class Answer_CreateLessonDto : AbstractDto<Answer, Answer_CreateLessonDto>
    {
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}
