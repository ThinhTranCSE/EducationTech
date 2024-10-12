using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;

namespace EducationTech.Business.Master.Interfaces;

public interface IQuizService : IService
{
    Task<QuizDto> CreateQuiz(Quiz_CreateRequestDto requestDto);
    Task<QuizDto> UpdateQuiz(int id, Quiz_UpdateRequestDto requestDto);

    //Task<bool> DeleteQuiz(int id, User? currentUser);

}
