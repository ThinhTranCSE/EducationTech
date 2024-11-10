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

    public async Task<bool> DeleteBranch(int id)
    {
        var branch = await _unitOfWork.Branches.GetAll()
            .Include(b => b.Specialities)
            .ThenInclude(s => s.Learners)
            .Include(b => b.Specialities)
            .ThenInclude(s => s.Courses)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (branch == null)
        {
            throw new Exception("Branch not found");
        }

        if (branch.Specialities.Any())
        {
            if (branch.Specialities.Any(s => s.Learners.Any()))
            {
                throw new Exception("Branch has specialities with learners");
            }

            if (branch.Specialities.Any(s => s.Courses.Any()))
            {
                throw new Exception("Branch has specialities with courses");
            }
        }


        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            // xoá luôn cả specialities
            _unitOfWork.Specialities.RemoveRange(branch.Specialities);
            _unitOfWork.Branches.Remove(branch);
            _unitOfWork.SaveChanges();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }

        return true;
    }
}
