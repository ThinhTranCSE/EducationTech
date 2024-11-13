using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Questions;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Quizzes
{
    public class QuizDto : AbstractDto<Quiz, QuizDto>
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int TimeLimit { get; set; }
        public ICollection<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
}
