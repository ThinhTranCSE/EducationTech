using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Answers
{
    public class Answer_UpdateRequestDto : AbstractDto<Answer, Answer_UpdateRequestDto>
    {
        public string? Content { get; set; }
        public int? Score { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
