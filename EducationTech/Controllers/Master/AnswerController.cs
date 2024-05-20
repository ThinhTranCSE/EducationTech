using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Answers;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master
{
    public class AnswerController : BaseController
    {
        private readonly IAnswerService _answerService;
        public AnswerController(EducationTechContext context, IAuthService authService, IAnswerService answerService) : base(context, authService)
        {
            _answerService = answerService;
        }

        [HttpPost]
        [Authorize(Policy = "UploadCourse")]
        public async Task<AnswerDto> CreateAnswer([FromBody] Answer_CreateRequestDto requestDto)
        {
            var answer = await _answerService.CreateAnswer(requestDto, CurrentUser);
            return answer;
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "UpdateCourse")]
        public async Task<AnswerDto> UpdateAnswer(int id, [FromBody]Answer_UpdateRequestDto requestDto)
        {
            var answer = await _answerService.UpdateAnswer(id, requestDto, CurrentUser);
            return answer;
        }
    }
}
