using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master;

public class QuizController : BaseController
{
    private readonly IQuizService _quizService;
    public QuizController(IQuizService quizService)
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
    public async Task<QuizDto> CreateQuiz([FromBody] Quiz_CreateRequest request)
    {
        var quiz = await _quizService.CreateQuiz(request);
        return quiz;
    }

    [HttpPatch("{id}")]
    public async Task<QuizDto> UpdateQuiz(int id, [FromBody] Quiz_UpdateRequest request)
    {
        var quiz = await _quizService.UpdateQuiz(request, id);
        return quiz;
    }

    //[HttpDelete("{id}")]
    //[Authorize(Policy = "DeleteCourse")]
    //public async Task DeleteQuiz(int id)
    //{
    //    await _quizService.DeleteQuiz(id, CurrentUser);
    //}
}
