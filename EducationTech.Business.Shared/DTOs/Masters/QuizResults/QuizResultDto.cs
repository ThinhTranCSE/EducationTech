using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.QuizResults;

public class QuizResultDto : AbstractDto<QuizResult, QuizResultDto>
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? Score { get; set; }
    public int? TimeTaken { get; set; }
}
