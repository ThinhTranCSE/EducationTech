namespace EducationTech.Business.Shared.DTOs.Masters.Quizzes;

public class Quiz_SubmitQuizRequest
{
    public int QuizResultId { get; set; }
    public ICollection<int> AnswerIds { get; set; } = new List<int>();
}
