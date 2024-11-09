using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Recommendation.Branches;

namespace EducationTech.Business.Recommendation.Interfaces;

public interface IBranchService : IService
{
    Task<List<BranchDto>> GetAll();
    Task<BranchDto> CreateBranch(CreateBranchRequest request);
}
