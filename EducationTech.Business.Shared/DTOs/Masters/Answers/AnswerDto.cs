using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Answers
{
    public class AnswerDto : AbstractDto<Answer, AnswerDto>
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int Score { get; set; }
        public string Content { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
