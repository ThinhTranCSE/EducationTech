using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;

namespace EducationTech.Business.Master.Interfaces;

public interface IQuizService : IService
{
    Task<QuizDto> CreateQuiz(Quiz_CreateRequest request);
    Task<QuizDto> UpdateQuiz(Quiz_UpdateRequest request, int id);

    Task<bool> DeleteQuiz(int id);
}
