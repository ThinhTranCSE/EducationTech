using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.Branches;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Recommendation
{
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IEnumerable<BranchDto>> GetBranches()
        {
            return await _branchService.GetAll();
        }

        [HttpPost]
        public async Task<BranchDto> CreateBranch([FromBody] CreateBranchRequest branch)
        {
            return await _branchService.CreateBranch(branch);
        }
    }
}
