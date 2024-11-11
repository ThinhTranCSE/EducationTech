using AutoMapper;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.CourseGroups;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Recommendation;

public class CourseGroupService : ICourseGroupService
{
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public CourseGroupService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CourseGroupDto> CreateCourseGroup(CourseGroup_CreateRequestDto requestDto)
    {
        var courseGroup = _mapper.Map<CourseGroup>(requestDto);

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.CourseGroups.Add(courseGroup);
            _unitOfWork.SaveChanges();
            transaction.Commit();

            return _mapper.Map<CourseGroupDto>(courseGroup);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> DeleteCourseGroup(int id)
    {
        var courseGroup = _unitOfWork.CourseGroups.GetAll().FirstOrDefault(x => x.Id == id);

        if (courseGroup == null)
        {
            throw new Exception("Course group not found");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.CourseGroups.Remove(courseGroup);
            _unitOfWork.SaveChanges();
            transaction.Commit();

            return true;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }

    }

    public async Task<List<CourseGroupDto>> GetAll()
    {
        var courseGroups = _unitOfWork.CourseGroups.GetAll().ToList();

        return _mapper.ProjectTo<CourseGroupDto>(courseGroups.AsQueryable()).ToList();
    }
}
