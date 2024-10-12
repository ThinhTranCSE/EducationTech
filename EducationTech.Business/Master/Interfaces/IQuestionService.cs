using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Questions;

namespace EducationTech.Business.Master.Interfaces
{
    public interface IQuestionService : IService
    {
        Task<QuestionDto> CreateQuestion(Question_CreateRequestDto requestDto);
        Task<QuestionDto> UpdateQuestion(int id, Question_UpdateRequestDto requestDto);
    }
}
