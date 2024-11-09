using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Recommendation.Specialities;

namespace EducationTech.Business.Recommendation.Interfaces;

public interface ISpecialityService : IService
{
    Task<SpecialityDto> CreateSpeciality(CreateSpecialityRequest request);
}
