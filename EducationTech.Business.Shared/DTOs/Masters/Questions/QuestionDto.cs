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
    public class QuestionDto : AbstractDto<Question, QuestionDto>
    {
        public int Id { get; set; }

        public int QuizId { get; set; }

        public string Content { get; set; }

        public ICollection<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }
}
