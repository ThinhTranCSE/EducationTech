using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Lessons;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master
{
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
    }
}
