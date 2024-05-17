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
    public class Question_CreateQuizDto : AbstractDto<Question, Question_CreateQuizDto>
    {
        public string Content { get; set; }
        public IEnumerable<Answer_CreateQuizDto> Answers { get; set; } = new List<Answer_CreateQuizDto>();

    }
}
