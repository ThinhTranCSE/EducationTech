using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Questions;

namespace EducationTech.Business.Master.Interfaces;

public interface IQuestionService : IService
{
    Task<QuestionDto> CreateQuestion(Question_CreateRequestDto request);
    Task<QuestionDto> UpdateQuestion(Question_UpdateRequestDto request, int id);
    Task<bool> DeleteQuestion(int id);
}
