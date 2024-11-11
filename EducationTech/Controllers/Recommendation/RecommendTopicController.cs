using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Recommendation;

public class RecommendTopicController : BaseController
{
    private readonly IRecomendTopicService _recomendTopicService;

    public RecommendTopicController(IRecomendTopicService recomendTopicService)
    {
        _recomendTopicService = recomendTopicService;
    }

    [HttpPost]
    public async Task<RecommendTopicDto> CreateTopic(RecommendTopic_CreateRequest requestDto)
    {
        var result = await _recomendTopicService.CreateTopic(requestDto);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<RecommendTopicDto> UpdateTopic(RecommendTopic_UpdateRequest requestDto, int id)
    {
        var result = await _recomendTopicService.UpdateTopic(requestDto, id);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteTopic(int id)
    {
        var result = await _recomendTopicService.DeleteTopic(id);
        return result;
    }
}
