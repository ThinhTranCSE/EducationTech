using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Lessons;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master.Interfaces
{
    public interface ILessonService : IService
    {
        Task<LessonDto> GetLessonById(int id, User? currentUser);
        Task<Lesson_ValidateQuizResponseDto> SubmitAnswers(Lesson_ValidateQuizRequestDto requestDto, User? currentUser);
        Task<LessonDto> CreateLesson(Lesson_CreateRequestDto requestDto, User? currentUser);
        Task<LessonDto> UpdateLesson(int id, Lesson_UpdateRequestDto requestDto, User? currentUser);

    }
}
