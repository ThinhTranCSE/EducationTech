using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Lessons
{
    public class Lesson_ValidateQuizRequestDto
    {
        public IEnumerable<int> AnswerIds { get; set; }
    }
}
