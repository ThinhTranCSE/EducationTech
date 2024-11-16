using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;

namespace EducationTech.Business.Recommendation.Interfaces;

public interface ILearningObjectService : IService
{
    Task<LearningObjectDto> GetLearningObjectById(int id);
    Task<LearningObjectDto> CreateLearningObject(LearningObject_CreateRequest request);
    Task<LearningObjectDto> UpdateLearningObject(LearningObject_UpdateRequest request, int id);
    Task<bool> DeleteLearningObject(int id);
}
