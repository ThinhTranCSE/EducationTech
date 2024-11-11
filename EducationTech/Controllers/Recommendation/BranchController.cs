using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.Branches;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Policy = "AdminAndInstructor")]
        [HttpGet]
        public async Task<IEnumerable<BranchDto>> GetBranches()
        {
            return await _branchService.GetAll();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<BranchDto> CreateBranch([FromBody] CreateBranchRequest branch)
        {
            return await _branchService.CreateBranch(branch);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteBranch(int id)
        {
            return await _branchService.DeleteBranch(id);
        }
    }
}
