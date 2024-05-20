using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Lessons;
using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.Business.Master.Interfaces
{
    public interface ILessonService : IService
    {
        Task<LessonDto> GetLessonById(int id, User? currentUser);
        Task<Lesson_ValidateQuizResponseDto> SubmitAnswers(Lesson_ValidateQuizRequestDto requestDto, User? currentUser);
        Task<LessonDto> CreateLesson(Lesson_CreateRequestDto requestDto, User? currentUser);
        Task<LessonDto> UpdateLesson(int id, Lesson_UpdateRequestDto requestDto, User? currentUser);
        Task<LessonDto> DeleteLesson(int id, User? currentUser);

    }
}
