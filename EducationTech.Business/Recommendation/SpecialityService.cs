using AutoMapper;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.Specialities;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Recommendation;

public class SpecialityService : ISpecialityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SpecialityService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<SpecialityDto> CreateSpeciality(CreateSpecialityRequest request)
    {
        if (_unitOfWork.Branches.GetById(request.BranchId) == null)
            throw new Exception("Branch not found");


        var speciality = new Speciality
        {
            Name = request.Name,
            BranchId = request.BranchId
        };

        _unitOfWork.Specialities.Add(speciality);
        _unitOfWork.SaveChanges();

        return _mapper.Map<SpecialityDto>(speciality);
    }

    public async Task<bool> DeleteSpeciality(int id)
    {
        var speciality = await _unitOfWork.Specialities.GetAll()
            .Include(x => x.Courses)
            .Include(x => x.Learners)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (speciality == null)
            throw new Exception("Speciality not found");

        if (speciality.Courses.Any())
        {
            throw new Exception("Speciality has courses");
        }

        if (speciality.Learners.Any())
        {
            throw new Exception("Speciality has learners");
        }

        _unitOfWork.Specialities.Remove(speciality);

        _unitOfWork.SaveChanges();

        return true;
    }
}
