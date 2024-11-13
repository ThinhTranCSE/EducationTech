using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Questions;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master;

public class QuestionController : BaseController
{
    private readonly IQuestionService _questionService;
    public QuestionController(EducationTechContext context, IAuthService authService, IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpPost]
    public async Task<QuestionDto> CreateQuestion([FromBody] Question_CreateRequestDto requestDto)
    {
        var question = await _questionService.CreateQuestion(requestDto);
        return question;
    }

    [HttpPatch("{id}")]
    public async Task<QuestionDto> UpdateQuestion(int id, [FromBody] Question_UpdateRequestDto requestDto)
    {
        var question = await _questionService.UpdateQuestion(requestDto, id);
        return question;
    }
}
