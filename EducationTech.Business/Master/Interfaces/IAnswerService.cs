using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Answers;

namespace EducationTech.Business.Master.Interfaces
{
    public interface IAnswerService : IService
    {
        Task<AnswerDto> CreateAnswer(Answer_CreateRequestDto request);
        Task<AnswerDto> UpdateAnswer(int id, Answer_UpdateRequestDto request);
    }
}
