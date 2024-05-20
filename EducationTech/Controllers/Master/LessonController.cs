using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Lessons;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master
{
    [Authorize]
    public class LessonController : BaseController
    {
        private readonly ILessonService _lessonService;
        public LessonController(EducationTechContext context, IAuthService authService, ILessonService lessonService) : base(context, authService)
        {
            _lessonService = lessonService;
        }

        [HttpGet("{id}")]
        public async Task<LessonDto> GetLessonById(int id)
        {
            var lesson = await _lessonService.GetLessonById(id, CurrentUser);
            return lesson;
        }

        [HttpPost("SubmitAnswers")]
        public async Task<Lesson_ValidateQuizResponseDto> SubmitAnswers(Lesson_ValidateQuizRequestDto requestDto)
        {
            var response = await _lessonService.SubmitAnswers(requestDto, CurrentUser);
            return response;
        }

        [HttpPost]
        [Authorize(Policy = "UploadCourse")]
        public async Task<LessonDto> CreateLesson([FromBody] Lesson_CreateRequestDto requestDto)
        {
            var lesson = await _lessonService.CreateLesson(requestDto, CurrentUser);
            return lesson;
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "UpdateCourse")]
        public async Task<LessonDto> UpdateLesson(int id, [FromBody] Lesson_UpdateRequestDto requestDto)
        {
            var lesson = await _lessonService.UpdateLesson(id, requestDto, CurrentUser);
            return lesson;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<LessonDto> DeleteLesson(int id)
        {
            var lesson = await _lessonService.DeleteLesson(id, CurrentUser);
            return lesson;
        }

    }
}
