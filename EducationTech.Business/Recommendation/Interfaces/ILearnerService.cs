using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Recommendation.Learners;

namespace EducationTech.Business.Recommendation.Interfaces;

public interface ILearnerService : IService
{
    Task<LearnerDto> UpdateLearner(UpdateLearnerRequest request);
}
