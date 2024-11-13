using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Questions;

public class Question_CreateRequestDto : AbstractDto<Question, Question_CreateRequestDto>
{
    public int QuizId { get; set; }
    public string Content { get; set; }
}
