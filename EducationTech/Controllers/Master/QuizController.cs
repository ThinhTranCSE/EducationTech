using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master;

public class QuizController : BaseController
{
    private readonly IQuizService _quizService;
    public QuizController(EducationTechContext context, IAuthService authService, IQuizService quizService)
    {
        _quizService = quizService;
    }

    //[HttpGet("{id}")]
    //public async Task<QuizDto> GetQuizById(int id)
    //{
    //    var quiz = await _quizService.GetQuizById(id, CurrentUser);
    //    return quiz;
    //}

    [HttpPost]
    [Authorize(Policy = "UploadCourse")]
    public async Task<QuizDto> CreateQuiz([FromBody] Quiz_CreateRequestDto requestDto)
    {
        var quiz = await _quizService.CreateQuiz(requestDto);
        return quiz;
    }

    [HttpPatch("{id}")]
    [Authorize(Policy = "UpdateCourse")]
    public async Task<QuizDto> UpdateQuiz(int id, [FromBody] Quiz_UpdateRequestDto requestDto)
    {
        var quiz = await _quizService.UpdateQuiz(id, requestDto);
        return quiz;
    }

    //[HttpDelete("{id}")]
    //[Authorize(Policy = "DeleteCourse")]
    //public async Task DeleteQuiz(int id)
    //{
    //    await _quizService.DeleteQuiz(id, CurrentUser);
    //}
}
