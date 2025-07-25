﻿using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Answers;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master;

public class AnswerController : BaseController
{
    private readonly IAnswerService _answerService;
    public AnswerController(EducationTechContext context, IAuthService authService, IAnswerService answerService)
    {
        _answerService = answerService;
    }

    [HttpPost]
    public async Task<AnswerDto> CreateAnswer([FromBody] Answer_CreateRequestDto requestDto)
    {
        var answer = await _answerService.CreateAnswer(requestDto);
        return answer;
    }

    [HttpPatch("{id}")]
    public async Task<AnswerDto> UpdateAnswer(int id, [FromBody] Answer_UpdateRequestDto requestDto)
    {
        var answer = await _answerService.UpdateAnswer(id, requestDto);
        return answer;
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteAnswer(int id)
    {
        var result = await _answerService.DeleteAnswer(id);
        return result;
    }
}
