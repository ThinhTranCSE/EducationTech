using AutoMapper;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.Branches;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Recommendation;

public class BranchService : IBranchService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public BranchService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<BranchDto>> GetAll()
    {
        var query = _unitOfWork.Branches.GetAll();

        query = query.Include(b => b.Specialities);

        var branches = await query.ToListAsync();

        return _mapper.ProjectTo<BranchDto>(branches.AsQueryable()).ToList();
    }

    public async Task<BranchDto> CreateBranch(CreateBranchRequest request)
    {
        var branch = new Branch
        {
            Name = request.Name
        };

        _unitOfWork.Branches.Add(branch);

        _unitOfWork.SaveChanges();

        return _mapper.Map<BranchDto>(branch);
    }
}
